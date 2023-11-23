using HistoryService.Entities;
using Microsoft.EntityFrameworkCore;

namespace HistoryService.Data.Repositories
{
    public class DoctorRepo : AbstractRepo, IBaseRepo<Doctor>
    {
        private readonly AppDbContext _context;

        public DoctorRepo(AppDbContext context, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _context = context;
        }

        public void ActiveOrDisactive(string id)
        {
            var entity = _context.Set<Doctor>().FirstOrDefault(p => p.Id == id);
            entity.ActiveOrDisable(CurrentUserName);
        }

        public Doctor Add(Doctor entity)
        {
            entity.Create(CurrentUserName);
            _context.Set<Doctor>().Add(entity);
            return entity;
        }

        public void Delete(string id)
        {
            var entity = _context.Set<Doctor>().FirstOrDefault(p => p.Id == id);
            _context.Set<Doctor>().Remove(entity);
        }

        public bool Exists(Func<Doctor, bool> predicate)
        {
            return _context.Set<Doctor>().Any(predicate);
        }

        public IEnumerable<Doctor> Get()
        {
            return _context.Set<Doctor>().AsNoTracking();
        }

        public Doctor Get(string id)
        {
            return _context.Set<Doctor>().FirstOrDefault(p => p.Id == id);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public Doctor Update(Doctor entity)
        {
            entity.Update(CurrentUserName);
            _context.Set<Doctor>().Update(entity);
            return entity;
        }
    }
}
