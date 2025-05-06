using FieldExpenseManager.FieldExpense.Domain.Entities;
using FieldExpenseManager.FieldExpense.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Persistence.SeedData
{
    public interface IUserDataSeeder
    {
        Task SeedUserDataAsync();
    }
    public class UserDataSeeder : IUserDataSeeder
    {
        private readonly FieldExpenseDbContext _dbContext;
        public UserDataSeeder(FieldExpenseDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public async Task SeedUserDataAsync()
        {
            if (!await _dbContext.Users.AnyAsync())
            {
                var adminUser = new User
                {
                    FirstName = "Burcu",
                    LastName = "Başak",
                    Email = "burcu@gmail.com",
                    PasswordHash = HashPassword("AdminSifre123!"),
                    PhoneNumber = "1234567890",
                    WorkPhoneNumber = "1234567892",
                    Role = Domain.Enums.UserRole.Admin,
                    IBAN = "TR123456789012345678901235",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,

                };
                var personelUser = new User
                {
                    FirstName = "Ali",  
                    LastName = "Veli",
                    Email="personel@gmail.com",
                    PasswordHash = HashPassword("PersonelSifre789."),
                    PhoneNumber = "1234567809",
                    WorkPhoneNumber = "1234567891",
                    Role = Domain.Enums.UserRole.Personnel,
                    IBAN = "TR123456789012345678901234",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

            var personelUser2 = new User
            {

                FirstName = "Ayşe",
                LastName = "Yılmaz",
                Email="personel2@gmail.com",
                PasswordHash = HashPassword("PersonelSifre7897."),
                PhoneNumber = "1234567809",
                WorkPhoneNumber = "1234567891",
                Role = Domain.Enums.UserRole.Personnel,
                IBAN = "TR123456789012345678901234",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

            };

                await _dbContext.Users.AddRangeAsync(adminUser, personelUser,personelUser2 );
                await _dbContext.SaveChangesAsync();

            }
        }
    }
}
