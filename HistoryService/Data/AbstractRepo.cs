namespace HistoryService.Data
{
    public abstract class AbstractRepo : IRepo
    {
        public string CurrentUserName { get; }
        public AbstractRepo(IHttpContextAccessor contextAccessor)
        {
            CurrentUserName = contextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        }
    }
}
