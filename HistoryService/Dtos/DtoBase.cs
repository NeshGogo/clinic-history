namespace HistoryService.Dtos
{
    public abstract class DtoBase
    {
        public string Id { get; set; }
        public DateTime RecordCreated { get; protected set; }
        public bool Active { get; protected set; } = true;
    }
}
