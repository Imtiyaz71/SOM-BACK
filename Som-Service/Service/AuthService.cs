using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Som_Models.Models;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using Som_Service.Interface;

namespace Som_Service.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        // Password ke SHA256 diye hash korar method
        private string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();
            foreach (var b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }

        public async Task<LoginResponse> LoginAsync(Login model)
        {
            var passwordHash = ComputeSha256Hash(model.password);

            using var connection = new SqlConnection(_connectionString);

            // 1. Login check
            var count = await connection.ExecuteScalarAsync<int>(
                "sp_Logsys",
                new { username = model.username, passwordHash = passwordHash },
                commandType: CommandType.StoredProcedure);

            if (count == 0)
                return null;

            // 2. Role fetch
     
            var userInfo = await connection.QueryFirstOrDefaultAsync<LoginResponse>(
              "sp_roleck",
              new { username = model.username },
              commandType: CommandType.StoredProcedure
          );

            var role = userInfo?.Role ?? "User";
        
            var fullname = userInfo?.Fullname ?? "Unknown";
  
            var username = userInfo?.Username ?? "Unknown";

            // 3. JWT Token generate
            var claims = new[]
            {
        new Claim(ClaimTypes.Role, role)
    

    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // 4. Return token + role
            return new LoginResponse
            {
                Token = tokenString,
                Role = role,
                Fullname=fullname,
                Username=username
            };
        }

    }
}
