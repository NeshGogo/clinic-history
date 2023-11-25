using HistoryService.Entities;
using Microsoft.EntityFrameworkCore;

namespace HistoryService.Data.Repositories
{
    public class PatientRepo : AbstractRepo, IBaseRepo<Patient>
    {
        private readonly AppDbContext _context;

        public PatientRepo(AppDbContext context, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _context = context;
        }

        public void ActiveOrDisactive(string id)
        {
            var entity = _context.Set<Patient>().FirstOrDefault(p => p.Id == id);
            entity.ActiveOrDisable(CurrentUserName);
        }

        public Patient Add(Patient entity)
        {
            entity.Create(CurrentUserName);
            _context.Set<Patient>().Add(entity);
            return entity;
        }

        public void Delete(string id)
        {
            var entity = _context.Set<Patient>().FirstOrDefault(p => p.Id == id);
            _context.Set<Patient>().Remove(entity);
        }

        public bool Exists(Func<Patient, bool> predicate)
        {
            return _context.Set<Patient>().Any(predicate);
        }

        public IEnumerable<Patient> Get()
        {
            return _context.Set<Patient>().AsNoTracking();
        }

        public Patient Get(string id)
        {
            return _context.Set<Patient>().FirstOrDefault(p => p.Id == id);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public Patient Update(Patient entity)
        {
            entity.Update(CurrentUserName);
            _context.Set<Patient>().Update(entity);
            return entity;
        }
    }
}
