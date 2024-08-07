

namespace Caretta.Core.BaseEntity
{
    public class AuditableEntityBase
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; } = Guid.NewGuid();
        public Guid? ModifiedBy { get; set; } //nullable
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedOn { get; set; } // nullable
    }
}
