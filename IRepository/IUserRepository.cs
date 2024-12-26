namespace Eduhub_MVC_Project.IRepository
{
    public interface IUserRepository
    {
        public void Register(User user);
        public User Login(string username, string password);
        public User GetUserById(int userId);
        void UpdateUser(User user);
    }
}