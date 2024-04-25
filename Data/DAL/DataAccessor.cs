using HWASP.Data.Context;

namespace HWASP.Data.DAL
{
    public class DataAccessor
    {
        public DataContext _context;
        public UserDao UserDao { get; private set; }
        public DataAccessor(DataContext context)
        {
            _context = context;
            UserDao = new(_context);
        }
    }
}
