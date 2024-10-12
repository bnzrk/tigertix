
namespace TigerTix.Web.Data.Entities
{
    public interface IUserRepository
    {
        // Creates an entry in the database's User table.
        void SaveUser(User user);

        // Updates an entry in the database's User table. Pass in a valid User entity returned by GetAllUsers, GetUserByID, etc.
        void UpdateUser(User user);

        // Deletes an entry in the database's User table. Pass in a valid User entity returned by GetAllUsers, GetUserByID, etc.
        void DeleteUser(User user);

        // Queries and returns all entries from the database's User table.
        IEnumerable<User> GetAllUsers();

        // Queries and returns a specific user from the database's User table by ID.
        User GetUserByID(int userID);

        // Saves all changes to the database and returns whether any entries were affected.
        bool SaveAll();
    }
}