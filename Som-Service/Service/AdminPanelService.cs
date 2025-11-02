using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Som_Models.Models;
using Som_Models.VW_Models;
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
    public class AdminPanelService : IAdminPanel
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public AdminPanelService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<VW_Response> DeleteCompanyModule(int id)
        {
            VW_Response res = new VW_Response();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", id);

                    // SP returns StatusCode + Message
                    var dbResult = await conn.QuerySingleAsync<VW_Response>(
                        "sp_DeleteCompanyModule",
                        param,
                        commandType: CommandType.StoredProcedure
                    );

                    res.StatusCode = dbResult.StatusCode;
                    res.Message = dbResult.Message;
                }
            }
            catch (Exception ex)
            {
                res.StatusCode = 500;
                res.Message = "Error while deleting company module: " + ex.Message;
            }

            return res;
        }

        public async Task<List<VW_ShowCompanyMenu>> GetCompanyMenu()
        {
            var result = new List<VW_ShowCompanyMenu>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    result = (await conn.QueryAsync<VW_ShowCompanyMenu>(
                        "sp_GetAllchildmenu",
                        commandType: CommandType.StoredProcedure
                    )).AsList();
                }
            }
            catch (Exception ex)
            {
                // Logging kora jabe ekhane
                throw new Exception("Error fetching company menu: " + ex.Message);
            }

            return result;
        }

        public async Task<List<VW_ShowCompanyMenu>> GetCompanyMenuByCompany(int compId)
        {
            var result = new List<VW_ShowCompanyMenu>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    result = (await conn.QueryAsync<VW_ShowCompanyMenu>(
                        "sp_GetAllchildmenuByCompId",
                        new { compId = compId },   // ✅ correct
                        commandType: CommandType.StoredProcedure
                    )).AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching company menu: " + ex.Message);
            }

            return result;
        }


        public async Task<List<VW_CompanyModule>> GetCompanyModule()
        {
            using var connection = new SqlConnection(_connectionString);

            var result = await connection.QueryAsync<VW_CompanyModule>(
                "sp_GetCompanyModuleList",
                commandType: CommandType.StoredProcedure
            );

            return result.AsList();
        }

        public async Task<AdminLoginResponse> LoginAsync(AdminPanel model)
        {
            var passwordHash = ComputeSha256Hash(model.Passwords);

            using var connection = new SqlConnection(_connectionString);

            // ✅ 1. Get user from stored procedure
            var user = await connection.QueryFirstOrDefaultAsync<AdminPanel>(
                "sp_AdminLogin_GetUser",
                new { userName = model.UserName, passwords = passwordHash },
                commandType: CommandType.StoredProcedure
            );

            // ❌ Invalid login
            if (user == null)
                return null;

            // ✅ 2. Create claims
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim("Fullname", user.FullName)
    };

            // ✅ 3. Generate JWT token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // ✅ 4. Return token + user info
            return new AdminLoginResponse
            {
                Token = tokenString,
                Fullname = user.FullName,
                Username = user.UserName
            };
        }

        private string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();
            foreach (var b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
     
    }
}
