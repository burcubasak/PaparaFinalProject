using FieldExpenseManager.FieldExpense.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Persistence.Configurations
{
    public class ExpensePaymentsConfiguration:IEntityTypeConfiguration<ExpensePayments>
    {
        public void Configure(EntityTypeBuilder<ExpensePayments> builder)
        {
            builder.ToTable("ExpensePaymentses");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.PaymentDate)
                .IsRequired();

            builder.Property(e => e.ErrorMessage)
                .HasMaxLength(500);

            builder.Property(e => e.TransactionId)
                .HasMaxLength(100);

            builder.HasOne(e=>e.Expense)
                .WithOne(e=>e.Payments)
                .HasForeignKey<ExpensePayments>(e => e.ExpenseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e=>e.IsActive).HasDefaultValue(true);
            builder.Property(e=>e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
