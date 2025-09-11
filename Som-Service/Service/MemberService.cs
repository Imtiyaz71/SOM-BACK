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
               
                if (model.IdenDocu == null || model.IdenDocu.Length == 0)
                    return "No file selected";

                // Ensure Uploads directory exists
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "MemberDocu");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                // Generate unique file name and save
                var docuname = $"{Guid.NewGuid()}_{model.IdenDocu.FileName}";
                var docupath = Path.Combine(uploadDir, docuname);
               
                using (var stream = new FileStream(docupath, FileMode.Create))
                {
                    await model.IdenDocu.CopyToAsync(stream);
                }
                if (model.Photo == null || model.Photo.Length == 0)
                    return "No file selected";
                var uploadDir2 = Path.Combine(Directory.GetCurrentDirectory(), "MemberPhoto");
                if (!Directory.Exists(uploadDir2))
                    Directory.CreateDirectory(uploadDir2);
                var photoname = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                var photopath = Path.Combine(uploadDir2, photoname);

                using (var stream = new FileStream(photopath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }
              

                // Set dates
                model.CreateDate = DateTime.Now.ToString("dd-MMMM-yyyy");
                model.UpdateDate = DateTime.Now.ToString("dd-MMMM-yyyy");
                string docpath = $"MemberDocu/{docuname}";
                string photoPath = $"MemberPhoto/{photoname}";
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
                parameters.Add("@Photo", photoPath);
                parameters.Add("@IdenDocu", docpath);
                parameters.Add("@CreateDate", model.CreateDate);
                parameters.Add("@CreateBy", model.CreateBy);
                parameters.Add("@UpdateDate", model.UpdateDate);

                // Call stored procedure using Dapper
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var result = await connection.ExecuteAsync(
                        "sp_InsertMember", // your stored procedure name
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
