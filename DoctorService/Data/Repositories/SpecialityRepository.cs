using DoctorService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Data.Repositories
{
    public class SpecialityRepository : IBaseRepository<Speciality>
    {
        private readonly AppDbContext _context;
        private readonly string _userName;

        public SpecialityRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userName = httpContextAccessor.HttpContext?.User?.Identity?.Name;
        }
        public void ActiveOrDisactive(string id)
        {
            var entity = _context.Set<Speciality>().FirstOrDefault(p => p.Id == id);
            entity.ActiveOrDisable(_userName);
        }

        public Speciality Add(Speciality entity)
        {
            entity.Create(_userName);
            _context.Set<Speciality>().Add(entity);
            return entity;
        }

        public void Delete(string id)
        {
            var entity = _context.Set<Speciality>().FirstOrDefault(p => p.Id == id);
            _context.Set<Speciality>().Remove(entity);
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
            entity.Update(_userName);
            _context.Set<Speciality>().Add(entity);
            return entity;
        }
    }
}
