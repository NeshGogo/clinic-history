using HistoryService.Entities;
using Microsoft.EntityFrameworkCore;

namespace HistoryService.Data.Repositories
{
    public class ClinicRecordRepo : AbstractRepo, IBaseRepo<ClinicRecord>
    {
        private readonly AppDbContext _context;

        public ClinicRecordRepo(AppDbContext context, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _context = context;
        }

        public void ActiveOrDisactive(string id)
        {
            var entity = _context.Set<ClinicRecord>().FirstOrDefault(p => p.Id == id);
            entity.ActiveOrDisable(CurrentUserName);
        }

        public ClinicRecord Add(ClinicRecord entity)
        {
            entity.Create(CurrentUserName);
            _context.Set<ClinicRecord>().Add(entity);
            return entity;
        }

        public void Delete(string id)
        {
            var entity = _context.Set<ClinicRecord>().FirstOrDefault(p => p.Id == id);
            _context.Set<ClinicRecord>().Remove(entity);
        }

        public bool Exists(Func<ClinicRecord, bool> predicate)
        {
            return _context.Set<ClinicRecord>().Any(predicate);
        }

        public IEnumerable<ClinicRecord> Get()
        {
            return _context.Set<ClinicRecord>().AsNoTracking();
        }

        public ClinicRecord Get(string id)
        {
            return _context.Set<ClinicRecord>().FirstOrDefault(p => p.Id == id);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public ClinicRecord Update(ClinicRecord entity)
        {
            entity.Update(CurrentUserName);
            _context.Set<ClinicRecord>().Update(entity);
            return entity;
        }
    }
}
