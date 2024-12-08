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
        private readonly ITicketRepository _ticketRepository;

        private readonly IApplicationUserRepository _userRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(UserManager<ApplicationUser> userManager, IApplicationUserRepository userRepository, IOrderRepository orderRepository, ITicketRepository ticketRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _ticketRepository = ticketRepository;
        }

        [HttpGet("/order/success")]
        public IActionResult Success([FromQuery] int orderId)
        {
            return View(orderId);
        }

        [HttpGet("/order/checkout")]
        public IActionResult Checkout([FromQuery] string userId, [FromQuery] int orderId)
        {
            Console.WriteLine($"User = {userId}");
            Console.WriteLine($"Order = {orderId}");

            CheckoutViewModel model = new CheckoutViewModel()
            {
                UserId = userId,
                OrderId = orderId
            };

            return View(model);
        }

        [HttpPost("/order/checkout")]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            Console.WriteLine("Checkout POST");
            Console.WriteLine($"User = {model.UserId}");
            Console.WriteLine($"Order = {model.OrderId}");

            // 1. Retrive order
            ApplicationUser orderUser = await _userRepository.GetUserByIdAsync(model.UserId);
            Order activeOrder = _userRepository.GetActiveOrder(model.UserId);
            if (orderUser == null || activeOrder != _orderRepository.GetOrderById(model.OrderId))
            {
                Console.WriteLine("BAD");
                return BadRequest();
            }

            Console.WriteLine("Processing payment...");

            // 2. Process payment
            var result = ProcessPayment(model);

            if (!result)
                return BadRequest();

            // 3. Complete order (move order to user's completed order list, set active to false)
            Console.WriteLine("Completing the order...");
            CompleteOrder(activeOrder);

            // 4. Redirect to payment success/summary page
            //return RedirectToAction("Success", "Order", new { orderId = activeOrder.Id });
            Console.WriteLine("Order completed successfully.");

            return RedirectToAction("Success", "Order", new { orderId = activeOrder.Id });
        }

        public bool ProcessPayment(CheckoutViewModel model) // payment info view model instead of int
        {
            return true;
        }

        public void CompleteOrder(Order order)
        {
            ApplicationUser orderUser = order.User;

            if (orderUser == null)
            {
                Console.WriteLine("Error completing order!");
                return;
            }

            // Remove active status and add all tickets to user
            order.IsActive = false;
            foreach (var t in order.Tickets)
            {
                orderUser.Tickets.Add(t);
                _ticketRepository.UpdateTicket(t);
            }
            _ticketRepository.SaveAll();

            // Initialize a new empty order for the user
            order.User.Orders.Add(new Order() { IsActive = true });
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
            model.UserId = currentUser.Id;
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
