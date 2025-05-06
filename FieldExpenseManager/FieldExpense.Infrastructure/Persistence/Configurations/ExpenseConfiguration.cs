using FieldExpenseManager.FieldExpense.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Persistence.Configurations
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("Expenses");

            builder.HasKey(e => e.Id);
            
            builder.Property(e=>e.Amount)
                   .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(e => e.Description)
                   .IsRequired().HasMaxLength(500);
        
            builder.Property(e => e.ExpenseDate)
                   .IsRequired();
            builder.Property(e => e.Location)
                     .HasMaxLength(200);
            builder.Property(e => e.RejectionReason)
                        .HasMaxLength(500);
            builder.Property(e=>e.IsActive)
                   .HasDefaultValue(true);
            builder.Property(e => e.CreatedAt)
                .IsRequired()
                     .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);


            builder.HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ExpenseCategory)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Attachments)
                .WithOne(a => a.Expense)
                .HasForeignKey(a => a.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Payments)
                .WithOne(p => p.Expense)
                .HasForeignKey<ExpensePayments>(p => p.ExpenseId);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.ApproverUserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
