﻿using Microsoft.AspNetCore.Identity;

namespace TigerTix.Web.Data.Entities
{
    public interface IApplicationUserRepository
    {
        // Creates a new user.
        Task<IdentityResult> SaveUserAsync(ApplicationUser user, string password);

        // Updates a specific user.
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);

        // Deletes a specific user.
        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);

        // Returns all users.
        IEnumerable<ApplicationUser> GetAllUsers();

        // Returns a specific user by id.
        Task<ApplicationUser> GetUserByIdAsync(string userId);

        // Returns a specific user by username.
        Task<ApplicationUser> GetUserByUsernameAsync(string username);

        // Returns all tickets owned by a specific user by id.
        Task<List<Ticket>> GetUserTickets(string userId);

        List<Order> GetUserOrders(string userId);

        Order GetActiveOrder(string userId);
    }
}