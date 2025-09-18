using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Som_Service.Service
{
    public class MemberService : IMemberService
    {
        private readonly string _connectionString;

        public MemberService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<VM_Member>> Getmember()
        {
            using var connection = new SqlConnection(_connectionString);

            var mem = await connection.QueryAsync<VM_Member>(
                "sp_memberList",                
                commandType: CommandType.StoredProcedure
            );

            return mem.ToList();
        }

        public async Task<VM_Member> GetmemberById(int Memno)
        {
            using var connection = new SqlConnection(_connectionString);

            var mem = await connection.QueryFirstOrDefaultAsync<VM_Member>(
                "sp_memberListByMemno",               
                new { Memno = Memno },           
                commandType: CommandType.StoredProcedure
            );

            return mem;
        }

        public async Task<string> SaveMember(Members model)
        {
            try
            {
                // Check Base64 strings
                if (string.IsNullOrEmpty(model.IdenDocu))
                    return "No identification document provided";
                if (string.IsNullOrEmpty(model.Photo))
                    return "No photo provided";

                // Ensure directories exist
                var uploadDirDoc = Path.Combine(Directory.GetCurrentDirectory(), "MemberDocu");
                var uploadDirPhoto = Path.Combine(Directory.GetCurrentDirectory(), "MemberPhoto");
                if (!Directory.Exists(uploadDirDoc)) Directory.CreateDirectory(uploadDirDoc);
                if (!Directory.Exists(uploadDirPhoto)) Directory.CreateDirectory(uploadDirPhoto);

                // Save IdenDocu
                var idenBytes = Convert.FromBase64String(model.IdenDocu.Split(',').Last());
                var docExt = ".pdf"; // default extension
                if (model.IdenDocu.Contains("application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
                    docExt = ".docx";
                else if (model.IdenDocu.Contains("application/msword"))
                    docExt = ".doc";

                var docName = $"{Guid.NewGuid()}{docExt}";
                var docPathFull = Path.Combine(uploadDirDoc, docName);
                await File.WriteAllBytesAsync(docPathFull, idenBytes);

                // Save Photo
                var photoBytes = Convert.FromBase64String(model.Photo.Split(',').Last());
                var photoExt = ".jpg"; // default extension
                if (model.Photo.Contains("image/png")) photoExt = ".png";
                else if (model.Photo.Contains("image/jpeg")) photoExt = ".jpg";

                var photoName = $"{Guid.NewGuid()}{photoExt}";
                var photoPathFull = Path.Combine(uploadDirPhoto, photoName);
                await File.WriteAllBytesAsync(photoPathFull, photoBytes);

                // Set dates
                model.CreateDate = DateTime.Now.ToString("dd-MMMM-yyyy");
                model.UpdateDate = DateTime.Now.ToString("dd-MMMM-yyyy");

                string docDbPath = $"MemberDocu/{docName}";
                string photoDbPath = $"MemberPhoto/{photoName}";

                // Map to DB parameters
                var parameters = new DynamicParameters();
                parameters.Add("@GivenName", model.GivenName);
                parameters.Add("@SureName", model.SureName);
                parameters.Add("@Phone", model.Phone);
                parameters.Add("@Email", model.Email);
                parameters.Add("@NiD", model.NiD);
                parameters.Add("@BiCNo", model.BiCNo);
                parameters.Add("@PassportNo", model.PassportNo);
                parameters.Add("@Nationality", model.Nationality);
                parameters.Add("@Gender", model.Gender);
                parameters.Add("@Father", model.Father);
                parameters.Add("@Mother", model.Mother);
                parameters.Add("@Address", model.Address);
                parameters.Add("@Photo", photoDbPath);
                parameters.Add("@IdenDocu", docDbPath);
                parameters.Add("@CreateDate", model.CreateDate);
                parameters.Add("@CreateBy", model.CreateBy);
                parameters.Add("@UpdateDate", model.UpdateDate);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    await connection.ExecuteAsync(
                        "sp_InsertMember",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return "Member saved successfully";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

    }
}
