namespace AccountService.Entities
{
    public abstract class BaseEntity
    {
        public DateTime RecordCreated { get; protected set; } = DateTime.Now;
        public string RecordCreatedBy { get; protected set; }
        public DateTime RecordUpdated { get; protected set; } = DateTime.Now;
        public string RecordUpdatedBy { get; protected set; }
        public bool Active { get; protected set; } = true;

        public void ActiveOrDisable(string recordby)
        {
            Active = !Active;
            Update(recordby);
        }

        public void Create(string recordby)
        {
            recordby = recordby.ToLower();
            RecordCreatedBy = recordby;
        }
        public void Update(string recordby)
        {
            recordby = recordby.ToLower();
            RecordCreatedBy = recordby;
        }
    }
}
