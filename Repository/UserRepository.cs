using Eduhub_MVC_Project.IRepository;
namespace Eduhub_MVC_Project.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public User Login(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }
        
        public User GetUserById(int userId) 
        { 
            return _context.Users.FirstOrDefault(u => u.UserId == userId); 
        }
        public void UpdateUser(User user) 
        { 
            _context.Users.Update(user); 
            _context.SaveChanges(); 
        }
    }
}