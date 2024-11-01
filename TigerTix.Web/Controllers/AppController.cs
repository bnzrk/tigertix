﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Security.Claims;
using TigerTix.Web.Data;
using TigerTix.Web.Data.Entities;
using TigerTix.Web.Models;
using System.Text.RegularExpressions;

namespace TigerTix.Web.Controllers
{
    public class AppController : Controller
    {
        private readonly IUserRepository _userRepository;

        private readonly IEventRepository _eventRepository;

        private readonly UserManager<ApplicationUser> _userManager;
        
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AppController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserRepository userRepository, IEventRepository eventRepository)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();

            if (user != null)
                Console.WriteLine("Logged in as: " + user.FirstName + " " + user.LastName);
            else
                Console.WriteLine("Not logged in.");

            return View();
        }

        public IActionResult Login()
        {
            // Redirect if a user is already logged in
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost("App/Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Redirect if a user is already logged in
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index");
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            // Redirect if a user is already logged in
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost("App/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Redirect if a user is already logged in
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index");
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
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index");
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

        // TODO: Obsolete. Remove
        public IActionResult ShowUsers()
        {
            var results = from u in _userRepository.GetAllUsers()
                          select u;

            return View(results.ToList());
        }


        public IActionResult ShowEvents() 
        {
            var results = from e in _eventRepository.GetAllEvents()
                          select e;

            return View(results.ToList());
        }

        public IActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost("App/CreateEvent")]
        public IActionResult CreateEvent(Event eventListing) 
        {
            if (ModelState.IsValid) {
                _eventRepository.SaveEvent(eventListing);
                _eventRepository.SaveAll();
            }
            return View();
        }

        // TODO: Rework to avoid call to database
        protected async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }
    }
}
