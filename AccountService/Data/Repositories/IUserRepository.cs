using AccountService.Entities;

namespace AccountService.Data.Repositories
{
    public interface IUserRepository
    {
        User Update(User user, string updatedBy);
        IEnumerable<User> GetAll();
        Task<User> FindById(string id);
        Task Delete(string id);
        Task<bool> SaveChanges();
        Task<bool> Exists(string id);
    }
}
