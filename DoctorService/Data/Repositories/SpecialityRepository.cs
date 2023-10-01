using DoctorService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Data.Repositories
{
    public class SpecialityRepository : AbstractRepository, IBaseRepository<Speciality>
    {
        private readonly AppDbContext _context;

        public SpecialityRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _context = context;
        }
        public void ActiveOrDisactive(string id)
        {
            var entity = _context.Set<Speciality>().FirstOrDefault(p => p.Id == id);
            entity.ActiveOrDisable(CurrentUserName);
        }

        public Speciality Add(Speciality entity)
        {
            entity.Create(CurrentUserName);
            _context.Set<Speciality>().Add(entity);
            return entity;
        }

        public void Delete(string id)
        {
            var entity = _context.Set<Speciality>().FirstOrDefault(p => p.Id == id);
            _context.Set<Speciality>().Remove(entity);
        }

        public bool Exists(Func<Speciality, bool> predicate)
        {
            return _context.Set<Speciality>().Any(predicate);
        }

        public IEnumerable<Speciality> Get()
        {
            return _context.Set<Speciality>().AsNoTracking();
        }

        public Speciality Get(string id)
        {
            return _context.Set<Speciality>().FirstOrDefault(p => p.Id == id);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public Speciality Update(Speciality entity)
        {
            entity.Update(CurrentUserName);
            _context.Set<Speciality>().Update(entity);
            return entity;
        }
    }
}
