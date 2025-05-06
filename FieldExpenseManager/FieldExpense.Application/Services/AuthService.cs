using AutoMapper;
using FieldExpenseManager.FieldExpense.Application.DTOs.Auth;
using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories;
using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.UnitOfWork;
using FieldExpenseManager.FieldExpense.Application.Interfaces.ServiceInterface;
using FieldExpenseManager.FieldExpense.Application.Token;
using FieldExpenseManager.FieldExpense.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FieldExpenseManager.FieldExpense.Application.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtSettingsOptions, ILogger<AuthService>logger)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettingsOptions.Value;
            _logger = logger;
        }
        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.FindByEmailAsync(loginDto.Email);
                if(user==null || !user.IsActive)
                {
                 _logger.LogWarning("User not found or inactive for email {Email}", loginDto.Email);
                   
                    return null;
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

                if (!isPasswordValid)
                {
                    _logger.LogWarning("Invalid password for email {Email}", loginDto.Email);
                    return null;
                }

               _logger.LogInformation("User {Email} logged in successfully", loginDto.Email);

                var token= GenerateJwtToken(user);
                return token;
            }
            catch (Exception ex )
            {
              _logger.LogError(ex,"An error occurred while logging in for email {Email}", loginDto.Email);
                return null;

            }
        }
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),

            };

            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes);
            _logger.LogInformation("Token will expire at {ExpirationTime}", expiresAt);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresAt,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
