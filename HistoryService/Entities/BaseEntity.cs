using System.ComponentModel.DataAnnotations;

namespace HistoryService.Entities
{
    public class BaseEntity
    {
        [MaxLength(36)]
        public string Id { get; set; }
        public DateTime RecordCreated { get; protected set; } = DateTime.Now;
        [MaxLength(200)]
        public string RecordCreatedBy { get; protected set; }
        public DateTime RecordUpdated { get; protected set; } = DateTime.Now;
        [MaxLength(200)]
        public string RecordUpdatedBy { get; protected set; }
        public bool Active { get; protected set; } = true;

        public void ActiveOrDisable(string recordby)
        {
            Active = !Active;
            Update(recordby);
        }

        public void Create(string recordby)
        {
            Id = Guid.NewGuid().ToString();
            recordby = recordby.ToLower();
            RecordCreatedBy = recordby;
            RecordUpdated = DateTime.Now;
            RecordUpdatedBy = recordby;
        }
        public void Update(string recordby)
        {
            recordby = recordby.ToLower();
            RecordUpdated = DateTime.Now;
            RecordUpdatedBy = recordby;
        }
    }
}
