using System.ComponentModel.DataAnnotations.Schema;

namespace FieldExpenseManager.FieldExpense.Domain.Entities
{
    public abstract class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } 
        public bool IsActive { get; set; } = true;
    }
}
