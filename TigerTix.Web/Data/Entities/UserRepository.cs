namespace TigerTix.Web.Data.Entities
{
    public class UserRepository : IUserRepository
    {
        private readonly TigerTixContext _context;

        public UserRepository(TigerTixContext context)
        {
            _context = context;
        }

        public void SaveUser(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAllUsers()
        {
            var users = from u in _context.Users select u;
            return users.ToList();
        }

        public User GetUserByID(int userID)
        {
            // Get first matching user entry or default if not found
            var user = (from u in _context.Users where u.Id == userID select u).FirstOrDefault();
            return user;
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            _context.Remove(user);
            _context.SaveChanges();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
