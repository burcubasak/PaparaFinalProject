using FieldExpenseManager.FieldExpense.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Persistence.Configurations
{
    public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
    {

        public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
        {
            builder.ToTable("ExpenseCategories");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
            builder.HasIndex(e => e.Name)
                .IsUnique();
            builder.Property(e => e.Description)
                .HasMaxLength(500);
            builder.Property(e => e.IsActive)
                .HasDefaultValue(true);
            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);

            builder.HasMany(c => c.Expenses)
                .WithOne(e => e.ExpenseCategory)
                .HasForeignKey(e => e.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
