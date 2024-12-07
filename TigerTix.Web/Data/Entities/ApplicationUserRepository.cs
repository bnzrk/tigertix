using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TigerTix.Web.Data.Entities
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TigerTixContext _context;

        public ApplicationUserRepository(UserManager<ApplicationUser> userManager, TigerTixContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> SaveUserAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<List<Ticket>> GetUserTickets(string userId)
        {
            return await _context.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Tickets)
            .Include(t => t.Event) // Include Event navigation property so this can be accessed
            .ToListAsync();
        }

        public List<Order> GetUserOrders(string userId)
        {
            return _context.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Orders)
            .ToList();
        }

        public Order GetActiveOrder(string userId)
        {
            var activeOrder = _context.Users
            .Include(u => u.Orders)
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Orders)
            .Include(o => o.User)
            .Include(o => o.Tickets)
            .ThenInclude(t => t.Event)
            .FirstOrDefault(o => o.IsActive);
            return activeOrder;
        }
    }
}
