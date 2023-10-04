using DoctorService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Data.Repositories
{
    public class UserRepository : AbstractRepository, IBaseRepository<User>
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _context = context;
        }

        public void ActiveOrDisactive(string id)
        {
            var entity = _context.Set<User>().FirstOrDefault(p => p.Id == id);
            entity.ActiveOrDisable(CurrentUserName);
        }

        public User Add(User entity)
        {
            entity.Create(CurrentUserName);
            _context.Set<User>().Add(entity);
            return entity;
        }

        public void Delete(string id)
        {
            var entity = _context.Set<User>().FirstOrDefault(p => p.Id == id);
            _context.Set<User>().Remove(entity);
        }

        public bool Exists(Func<User, bool> predicate)
        {
            return _context.Set<User>().Any(predicate);
        }

        public IEnumerable<User> Get()
        {
            return _context.Set<User>().AsNoTracking();
        }

        public User Get(string id)
        {
            return _context.Set<User>().FirstOrDefault(p => p.Id == id);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public User Update(User entity)
        {
            entity.Update(CurrentUserName);
            _context.Set<User>().Update(entity);
            return entity;
        }
    }
}
