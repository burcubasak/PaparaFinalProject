using FieldExpenseManager.FieldExpense.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Persistence.DbContext
{
    public class FieldExpenseDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public FieldExpenseDbContext(DbContextOptions<FieldExpenseDbContext> options) : base(options)
        {
        }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<ExpenseAttachment> ExpenseAttachments { get; set; }
        public DbSet<ExpensePayments> ExpensePaymentses { get; set; }
        public DbSet<User> Users { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
