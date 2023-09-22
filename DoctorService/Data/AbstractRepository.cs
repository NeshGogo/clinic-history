using Microsoft.AspNetCore.Http;

namespace DoctorService.Data
{
    public abstract class AbstractRepository : IRepository
    {
        public string CurrentUserName { get; }
        public AbstractRepository(IHttpContextAccessor contextAccessor)
        {
            CurrentUserName = contextAccessor.HttpContext?.User?.Identity?.Name;
        }
    }
}
