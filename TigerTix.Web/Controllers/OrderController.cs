using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using TigerTix.Web.Data.Entities;
using TigerTix.Web.Models;

namespace TigerTix.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        private readonly IApplicationUserRepository _userRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(UserManager<ApplicationUser> userManager, IApplicationUserRepository userRepository, IOrderRepository orderRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        public IActionResult Success() // Use order view model
        {
            return View();
        }

        public IActionResult Checkout() // Use checkout view model
        {
            return View();
        }

        [HttpPost("/order/checkout")]
        public IActionResult Checkout(int model, string userId, int orderId) // user and order id in model instead
        {
            // 1. Retrive order
            Order activeOrder = _userRepository.GetActiveOrder(userId);
            if (activeOrder != _orderRepository.GetOrderById(orderId))
            {
                return BadRequest();
            }

            // 2. Process payment
            var result = ProcessPayment(model);

            if (!result)
                return BadRequest(); // FIXME: return page with error message

            // 3. Complete order (move order to user's completed order list, set active to false)
            CompleteOrder(activeOrder);

            // 4. TODO: Initialize a new empty order

            // 5. Redirect to payment success/summary page
            return RedirectToAction("Success", "Order", new { orderId = activeOrder.Id });
        }

        public bool ProcessPayment(int model) // payment info view model instead of int
        {
            return true;
        }

        public void CompleteOrder(Order order)
        {
            order.IsActive = false;
            _orderRepository.UpdateOrder(order);
        }

        public async Task<IActionResult> Cart()
        {
            // Make sure a user is logged in
            if (!UserIsLoggedIn())
                return RedirectToAction("Index", "Home");

            // If there is no active order, initialize one.
            ApplicationUser currentUser = await GetCurrentUserAsync();
            Order activeOrder = _userRepository.GetActiveOrder(currentUser.Id);

            // Create a view model from the active order
            OrderViewModel model = new OrderViewModel();
            model.Id = activeOrder.Id;
            model.Tickets = activeOrder.Tickets.Select(ticket => new OrderTicketViewModel
            {
                Id = ticket.Id,
                EventName = ticket.Event.Name,
                EventDate = ticket.Event.Date,
                Section = ticket.Section,
                Row = ticket.Row,
                SeatNumber = ticket.SeatNumber,
                Price = ticket.Price
            }).ToList();

            return View(model);
        }

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

        protected async Task<Order> InitializeUserOrder(ApplicationUser user)
        {
            Order order = new Order();
            order.IsActive = true;
            user.Orders.Add(order);
            await _userRepository.UpdateUserAsync(user);
            return order;
        }
    }
}
