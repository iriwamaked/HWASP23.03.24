using HWASP.Data.Context;
using HWASP.Data.Entities;

namespace HWASP.Data.DAL
{
    public class UserDao
    {
        private readonly DataContext _context;
        public UserDao(DataContext context)
        {
            _context = context;
        }
        public bool IsEmailFree(String email)
        {
            return _context.
                Users.
                Where(u=>u.Email==email)
                .Any();
        }

        public void RegisterUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
