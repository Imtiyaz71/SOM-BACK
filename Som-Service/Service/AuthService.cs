using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Som_Models.Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<CompanyInfo> CompanyInfo()
        {
            using var connection = new SqlConnection(_connectionString);
            try
            {
                var companyInfo = await connection.QueryFirstOrDefaultAsync<CompanyInfo>(
             "sp_companyInfo",
             
             commandType: CommandType.StoredProcedure
         );
                return new CompanyInfo
                {
                    Id = companyInfo.Id,
                    cName = companyInfo.cName,
                    cPhone = companyInfo.cPhone,
                    cEmail = companyInfo.cEmail,
                    cWebsite = companyInfo.cWebsite,
                    cAddress = companyInfo.cAddress,
                    cLogo = companyInfo.cLogo
                };
            }
            catch (Exception)
            {

                throw;
            }
            
            return new CompanyInfo
            {
                Id = 0,
                cName = "",
                cPhone = "",
                cEmail = "",
                cWebsite = "",
                cAddress = "",
                cLogo = ""
            };
        }

        public async Task<string> SaveCompany(CompanyInfo info)
        {
            try
            {
               
                if (string.IsNullOrEmpty(info.cLogo))
                    return "No photo provided";

                // ensure directories
             
                var uploadDirPhoto = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
               
                Directory.CreateDirectory(uploadDirPhoto);

              

                // Save Photo
                var photoBase64 = info.cLogo.Contains(",")
                    ? info.cLogo.Split(',').Last()
                    : info.cLogo;
                var photoBytes = Convert.FromBase64String(photoBase64);
                var photoExt = ".jpg";
                if (info.cLogo.Contains("image/png")) photoExt = ".png";
                else if (info.cLogo.Contains("image/jpeg")) photoExt = ".jpg";
                var photoName = $"{Guid.NewGuid()}{photoExt}";
                var photoPathFull = Path.Combine(uploadDirPhoto, photoName);
                await File.WriteAllBytesAsync(photoPathFull, photoBytes);

                // Set dates (DateTime হিসেবে)
                info.createAt = DateTime.Now.ToString("dd-MM-yyyy");
              
                string photoDbPath = $"Uploads/{photoName}";
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@cname", info.cName, DbType.String);
                parameters.Add("@cphone", info.cPhone, DbType.String);
                parameters.Add("@cemail", info.cEmail, DbType.String);
                parameters.Add("@cwebsite", info.cWebsite, DbType.String);
                parameters.Add("@caddress", info.cAddress, DbType.String);
                parameters.Add("@clogo", photoDbPath, DbType.String);
                parameters.Add("@createat", info.createAt, DbType.String);
            

                await connection.ExecuteAsync(
                    "sp_addcompany",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return "Somity Information Add Your Password is 123";
            }
            catch (Exception ex)
            {
                // Optional: log error
                return $"Error: {ex.Message}";
            }
        }
    }
}
