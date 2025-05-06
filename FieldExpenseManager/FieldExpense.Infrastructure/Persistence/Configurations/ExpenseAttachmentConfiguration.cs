using FieldExpenseManager.FieldExpense.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Persistence.Configurations
{
    public class ExpenseAttachmentConfiguration : IEntityTypeConfiguration<ExpenseAttachment>
    {
        public void Configure(EntityTypeBuilder<ExpenseAttachment> builder)
        {
            builder.ToTable("ExpenseAttachments");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FileName)
                   .IsRequired().HasMaxLength(255);

            builder.Property(e => e.FilePathOrUrl)
                   .IsRequired();
            builder.Property(e => e.ContentType)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(e => e.FileSize)
                     .IsRequired();
         
        }
    }
}
