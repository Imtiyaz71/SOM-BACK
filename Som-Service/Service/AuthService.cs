using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net.NetworkInformation;
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
    try
    {
        // 🔹 Hash password
        var passwordHash = ComputeSha256Hash(model.password);

        using var connection = new SqlConnection(_connectionString);

        // 🔹 Step 1: Login check
        var count = await connection.ExecuteScalarAsync<int>(
            "sp_Logsys",
            new { username = model.username, passwordHash = passwordHash },
            commandType: CommandType.StoredProcedure);

        if (count == 0)
        {
            return new LoginResponse
            {
                Token = null,
                Role = null,
                Message = "Invalid username or password"
            };
        }

        // 🔹 Step 2: Role fetch
        var userInfo = await connection.QueryFirstOrDefaultAsync<LoginResponse>(
            "sp_roleck",
            new { username = model.username },
            commandType: CommandType.StoredProcedure
        );

        if (userInfo == null)
        {
            return new LoginResponse
            {
                Token = null,
                Role = null,
                Message = "User information not found"
            };
        }

        int cid = Convert.ToInt32(userInfo.cId);

        // 🔹 Step 3: Check client status
        var status = await connection.QueryFirstOrDefaultAsync<int>(
            "SELECT cStatus FROM ClientStatus WHERE clientId = @clientId",
            new { clientId = cid },
            commandType: CommandType.Text
        );

        if (status == 0)
        {
            return new LoginResponse
            {
                Token = null,
                Role = null,
                Fullname = userInfo.Fullname,
                Username = userInfo.Username,
                cName = userInfo.cName,
                cId = cid,
                Message = "Your subscription validity has expired"
            };
        }

        // 🔹 Step 4: Prepare JWT claims
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userInfo.Username ?? "Unknown"),
            new Claim(ClaimTypes.Role, userInfo.Role ?? "User"),
            new Claim("Fullname", userInfo.Fullname ?? "Unknown"),
            new Claim("ClientName", userInfo.cName ?? "Unknown"),
            new Claim("ClientId", cid.ToString())
        };

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

        // 🔹 Step 5: Return success response
        return new LoginResponse
        {
            Token = tokenString,
            Role = userInfo.Role,
            Fullname = userInfo.Fullname,
            Username = userInfo.Username,
            cName = userInfo.cName,
            cId = cid,
            Message = "Login successful"
        };
    }
    catch (Exception ex)
    {
        // 🔹 Step 6: Handle unexpected errors
        return new LoginResponse
        {
            Token = null,
            Role = null,
            Message = "Login failed: " + ex.Message
        };
    }
}

        public async Task<CompanyInfo> CompanyInfo(int cid)
        {
            using var connection = new SqlConnection(_connectionString);
            try
            {
                var companyInfo = await connection.QueryFirstOrDefaultAsync<CompanyInfo>(
             "sp_companyInfo",
            new { cid=cid},
             
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
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
              
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
                

                    parameters.Add("@compId", info.Id, DbType.Int32);
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

        public async Task<List<CompanyInfo>> GetAllCompanyInfo()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
               

                var result = await conn.QueryAsync<CompanyInfo>(
                    "sp_allcompanyInfo",
                    
                    commandType: CommandType.StoredProcedure
                );

                return result.AsList();
            }
        }
    }
}
