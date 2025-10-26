
namespace Domain.Entities
{
    public abstract class BaseEntity<T>
    {
        public required T Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
