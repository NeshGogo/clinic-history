using AccountService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Delete(string id)
        {
            var entity = await _context.Set<User>().FindAsync(id);
            _context.Set<User>().Remove(entity);
        }

        public Task<bool> Exists(string id)
        {
            return _context.Set<User>().AnyAsync(p => p.Id == id);
        }

        public async Task<User> FindById(string id)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Set<User>().AsNoTracking();
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public User Update(User user, string updatedBy)
        {
            user.RecordUpdated = DateTime.Now;
            user.RecordCreatedBy = updatedBy;
            _context.Set<User>().Update(user);
            return user;
        }
    }
}
