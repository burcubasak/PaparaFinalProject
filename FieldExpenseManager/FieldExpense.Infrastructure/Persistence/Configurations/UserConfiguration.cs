using FieldExpenseManager.FieldExpense.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                .IsRequired().HasMaxLength(50);
            builder.Property(u => u.LastName)
                .IsRequired().HasMaxLength(50);
            builder.Property(u => u.Email)
                .IsRequired().HasMaxLength(100);
            builder.HasIndex(u => u.Email)
                .IsUnique();
            builder.Property(u => u.PasswordHash)
                .IsRequired();
            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(15);
            builder.Property(u => u.WorkPhoneNumber)
                .HasMaxLength(15);
            builder.Property(u => u.IBAN)
                .HasMaxLength(26);
            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);
            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(u => u.UpdatedAt)
                .IsRequired(false);

            builder.HasMany(u => u.Expenses)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
