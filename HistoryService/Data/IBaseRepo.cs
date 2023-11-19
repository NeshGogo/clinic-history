namespace HistoryService.Data
{
    public interface IBaseRepo<T>
    {
        IEnumerable<T> Get();
        T Get(string id);
        T Update(T entity);
        T Add(T entity);
        void Delete(string id);
        void ActiveOrDisactive(string id);
        Task<bool> SaveChanges();
        bool Exists(Func<T, bool> predicate);
    }
}
