﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TigerTix.Web.Data.Entities;
using TigerTix.Web.Models;
using System.Text.RegularExpressions;

namespace TigerTix.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IApplicationUserRepository _userRepository;

        private readonly ITicketRepository _ticketRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IApplicationUserRepository userRepository, ITicketRepository ticketRepository)
        {
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /************************
         * User Profile
         * **********************/

        public async Task<IActionResult> Profile()
        {
            if (!UserIsLoggedIn())
                return RedirectToAction("Index", "Home");

            ProfileViewModel model = new ProfileViewModel();
            ApplicationUser user = await GetCurrentUserAsync();

            if (user == null)
                return RedirectToAction("Index", "Home");

            model.UserName = user.UserName;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;

            return View(model);
        }

        public IActionResult Tickets()
        {
            UserTicketViewModel model = new UserTicketViewModel();
            model.DateTime = DateTime.Now;
            model.Number = 1;
            model.Section = "A1";
            model.Row = 1;
            model.EventName = "Event Name";
            model.SeatNumber = 1;

            List<UserTicketViewModel> list = new List<UserTicketViewModel> { model, model, model, model, model };

            return View(list);
        }

        /************************
         * Login and Registration
         * **********************/

        public IActionResult Login()
        {
            // Redirect if a user is already logged in
            if (UserIsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost("User/Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Redirect if a user is already logged in
            if (UserIsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Register()
        {
            // Redirect if a user is already logged in
            if (UserIsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost("User/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Redirect if a user is already logged in
            if (UserIsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            // Ensure valid CUID format in model
            string pattern = @"^C\d{6}$";
            if (!Regex.IsMatch(model.UserName ?? "", pattern))
            {
                ModelState.AddModelError("UserName", "Invalid CUID");
            }

            // Ensure all required fields are filled in model
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Attempt to create the new user and sign in on success
            ApplicationUser user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await _userRepository.SaveUserAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            // Grab all error messages from attempting to create the user and add to model state
            foreach (var error in result.Errors)
            {
                if (error.Code == "DuplicateUserName")
                {
                    // Reword the duplicate username error message
                    ModelState.AddModelError("UserName", "An account with this CUID is already registered.");
                }
                else
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /************************
         * Helper Functions
         * **********************/

        protected async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        protected bool UserIsLoggedIn()
        {
            return User.Identity?.IsAuthenticated == true;
        }
    }
}
