using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Som_Service.Service
{
    public class UserInfoService: IUserInfo
    {
        private readonly string _connectionString;

        public UserInfoService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<UserInfoBasic>> GetUserInfoBasic()
        {
            using var connection = new SqlConnection(_connectionString);

            var user = await connection.QueryAsync<UserInfoBasic>(
                "sp_UserInfoBasic",                 // Stored procedure name
                commandType: CommandType.StoredProcedure
            );

            return user.ToList();
        }
        public async Task<UserInfoBasic> GetUserInfoBasicByUser(string Username)
        {
            using var connection = new SqlConnection(_connectionString);

            var user = await connection.QueryFirstOrDefaultAsync<UserInfoBasic>(
                "sp_UserInfoBasicByUser",                     // Stored procedure name
                new { users = Username },            // Parameter object (name must match SP param)
                commandType: CommandType.StoredProcedure
            );

            return user;
        }
        public async Task<UserInfoEducation> GetUserInfoEducationByUser(string Username)
        {
            using var connection = new SqlConnection(_connectionString);

            var user = await connection.QueryFirstOrDefaultAsync<UserInfoEducation>(
                "sp_UserEducationByUser",                     // Stored procedure name
                new { users = Username },            // Parameter object (name must match SP param)
                commandType: CommandType.StoredProcedure
            );

            return user;
        }
        public async Task<VW_UserInfo> GetUserInfo(string Username)
        {
            using var connection = new SqlConnection(_connectionString);

            var user = await connection.QueryFirstOrDefaultAsync<VW_UserInfo>(
                "sp_userinfoall",
                new { username = Username }, // parameter name must match SP
                commandType: CommandType.StoredProcedure
            );

            return user;
        }
      

        public async Task<string> SaveUserInfoBasic(UserInfoBasic user)
        {
            string msg = "";


            try
            {
                using var connection = new SqlConnection(_connectionString);
                user.CreateDate = DateTime.Now.ToString("dd-MMMM-yyyy");
                var parameters = new DynamicParameters();
                parameters.Add("@fullname", user.FullName);
                parameters.Add("@phone", user.Phone);
                parameters.Add("@email", user.Email);
                parameters.Add("@address", user.Address);
                parameters.Add("@nid", user.NiD);
                parameters.Add("@father", user.Father);
                parameters.Add("@mother", user.Mother);
                parameters.Add("@username", user.Username);
                parameters.Add("@createdate", user.CreateDate);
                parameters.Add("@updatedate", user.UpdateDate);

                var result = await connection.ExecuteScalarAsync<int>(
                    "sp_saveUserBasic",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result == -1)
                {
                    msg = "Username already exists!";
                }

                msg = "User saved successfully";

            }
            catch (Exception ex)
            {

                msg = "Invalid Input";
            }
            return msg;
        }
        public async Task<string> SaveUserPhoto(IFormFile file, UserPhoto user)
        {
            string msg = "";

            try
            {
                if (file == null || file.Length == 0)
                    return "No file selected";

                // Ensure Uploads directory exists
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                // Generate unique file name and save
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                using var connection = new SqlConnection(_connectionString);

                // Set dates
                user.CreateDate = DateTime.Now.ToString("dd-MMMM-yyyy");
                user.UpdateDate = DateTime.Now.ToString("dd-MMMM-yyyy");
                user.Photo = $"Uploads/{fileName}";

                var parameters = new DynamicParameters();
                parameters.Add("@Username", user.Username);
                parameters.Add("@Photo", user.Photo);
                parameters.Add("@CreateDate", user.CreateDate);
                parameters.Add("@UpdateDate", user.UpdateDate);

                // Execute SP
                var result = await connection.ExecuteScalarAsync<int>(
                    "sp_saveuserphoto",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                // Result handling (optional, SP can return -1 if username exists)
                if (result == -1)
                {
                    msg = "Username already exists!";
                }
                else
                {
                    msg = "Photo uploaded and user saved successfully!";
                }
            }
            catch (Exception ex)
            {
                msg = "Invalid Input: " + ex.Message;
            }

            return msg;
        }

        public async Task<string> SaveUserInfoEducation(UserInfoEducation user)
        {
            string msg = "";


            try
            {
                using var connection = new SqlConnection(_connectionString);
                user.CreateDate = DateTime.Now.ToString("dd-MMMM-yyyy");
                var parameters = new DynamicParameters();
                parameters.Add("@degree", user.Degree);
                parameters.Add("@FieldOfStudy", user.FieldOfStudy);
                parameters.Add("@SchoolName", user.SchoolName);
                parameters.Add("@StartDate", user.StartDate);
                parameters.Add("@EndDate", user.EndDate);
                parameters.Add("@Description", user.Description);
                parameters.Add("@Username", user.Username);
                parameters.Add("@CreateDate", user.CreateDate);
                parameters.Add("@UpdateDate", user.UpdateDate);

                var result = await connection.ExecuteScalarAsync<int>(
                    "sp_saveusereducation",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result == -1)
                {
                    msg = "Username already exists!";
                }

                msg = "User saved successfully";

            }
            catch (Exception ex)
            {

                msg = "Invalid Input";
            }
            return msg;
        }

        public async Task<string> EditUserInfo(IFormFile file, VW_UserInfo user)
        {
            try
            {
                string photoPath = user.Photo; // existing path

                // === File upload ===
                if (file != null && file.Length > 0)
                {
                    // Reuse logic from SaveUserPhoto
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                    if (!Directory.Exists(uploadDir))
                        Directory.CreateDirectory(uploadDir);

                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var fullPath = Path.Combine(uploadDir, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    photoPath = $"Uploads/{fileName}";
                }

                // === Update user info using SP ===
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();

                parameters.Add("@fullname", user.FullName ?? string.Empty, DbType.String, size: 100);
                parameters.Add("@phone", user.Phone ?? string.Empty, DbType.String, size: 100);
                parameters.Add("@email", user.Email ?? string.Empty, DbType.String, size: 100);
                parameters.Add("@address", user.Address ?? string.Empty, DbType.String, size: 255);
                parameters.Add("@nid", user.NiD ?? string.Empty, DbType.String, size: 100);
                parameters.Add("@father", user.Father ?? string.Empty, DbType.String, size: 100);
                parameters.Add("@mother", user.Mother ?? string.Empty, DbType.String, size: 100);
                parameters.Add("@degree", user.Degree ?? string.Empty, DbType.String, size: 150);
                parameters.Add("@fieldofstudy", user.FieldOfStudy ?? string.Empty, DbType.String, size: 150);
                parameters.Add("@schoolname", user.SchoolName ?? string.Empty, DbType.String, size: 100);
                parameters.Add("@description", user.Description ?? string.Empty, DbType.String, size: 255);
                parameters.Add("@startdate", user.StartDate ?? string.Empty, DbType.String, size: 100);
                parameters.Add("@enddate", user.EndDate ?? string.Empty, DbType.String, size: 100);
                parameters.Add("@photo", photoPath ?? string.Empty, DbType.String);
                parameters.Add("@username", user.Username, DbType.String, size: 50);

                await connection.ExecuteAsync(
                    "sp_useredit",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return "User info updated successfully!";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

    }
}
