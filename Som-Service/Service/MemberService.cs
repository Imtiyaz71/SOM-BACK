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

        public async Task<string> EditMember(Members model)
        {
            using var connection = new SqlConnection(_connectionString);

            // Ensure directories exist
            var uploadDirDoc = Path.Combine(Directory.GetCurrentDirectory(), "MemberDocu");
            var uploadDirPhoto = Path.Combine(Directory.GetCurrentDirectory(), "MemberPhoto");
            if (!Directory.Exists(uploadDirDoc)) Directory.CreateDirectory(uploadDirDoc);
            if (!Directory.Exists(uploadDirPhoto)) Directory.CreateDirectory(uploadDirPhoto);

            // পুরনো ডেটা আগে নিয়ে আসি
            var existing = await connection.QueryFirstOrDefaultAsync<VM_Member>(
                "sp_memberListByMemno",
                new { Memno = model.MemNo },
                commandType: CommandType.StoredProcedure
            );

            if (existing == null)
                return "Member not found";

            // ফাইল path variables
            string docDbPath = existing.IdenDocu; // default পুরনোটা
            string photoDbPath = existing.Photo;  // default পুরনোটা

            // যদি নতুন IdenDocu আসে → সেভ করো
            if (!string.IsNullOrEmpty(model.IdenDocu))
            {
                var idenBytes = Convert.FromBase64String(model.IdenDocu.Split(',').Last());
                var docExt = ".pdf"; // default extension
                if (model.IdenDocu.Contains("application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
                    docExt = ".docx";
                else if (model.IdenDocu.Contains("application/msword"))
                    docExt = ".doc";

                var docName = $"{Guid.NewGuid()}{docExt}";
                var docPathFull = Path.Combine(uploadDirDoc, docName);
                await File.WriteAllBytesAsync(docPathFull, idenBytes);

                docDbPath = $"MemberDocu/{docName}"; // DB path update
            }

            // যদি নতুন Photo আসে → সেভ করো
            if (!string.IsNullOrEmpty(model.Photo))
            {
                var photoBytes = Convert.FromBase64String(model.Photo.Split(',').Last());
                var photoExt = ".jpg"; // default extension
                if (model.Photo.Contains("image/png")) photoExt = ".png";
                else if (model.Photo.Contains("image/jpeg")) photoExt = ".jpg";

                var photoName = $"{Guid.NewGuid()}{photoExt}";
                var photoPathFull = Path.Combine(uploadDirPhoto, photoName);
                await File.WriteAllBytesAsync(photoPathFull, photoBytes);

                photoDbPath = $"MemberPhoto/{photoName}";
            }

            model.GivenName = string.IsNullOrWhiteSpace(model.GivenName) ? existing.GivenName : model.GivenName;
            model.SureName = string.IsNullOrWhiteSpace(model.SureName) ? existing.SureName : model.SureName;
            model.Phone = string.IsNullOrWhiteSpace(model.Phone) ? existing.Phone : model.Phone;
            model.Email = string.IsNullOrWhiteSpace(model.Email) ? existing.Email : model.Email;
            model.NiD = string.IsNullOrWhiteSpace(model.NiD) ? existing.NiD : model.NiD;
            model.BiCNo = string.IsNullOrWhiteSpace(model.BiCNo) ? existing.BiCNo : model.BiCNo;
            model.PassportNo = string.IsNullOrWhiteSpace(model.PassportNo) ? existing.PassportNo : model.PassportNo;
            model.Nationality = string.IsNullOrWhiteSpace(model.Nationality) ? existing.Nationality : model.Nationality;
            model.Father = string.IsNullOrWhiteSpace(model.Father) ? existing.Father : model.Father;
            model.Mother = string.IsNullOrWhiteSpace(model.Mother) ? existing.Mother : model.Mother;
            model.Address = string.IsNullOrWhiteSpace(model.Address) ? existing.Address : model.Address;

            model.IdenDocu = docDbPath;
            model.Photo = photoDbPath;
            model.Gender = model.Gender == 0 ? existing.GenderId : model.Gender;
            model.UpdateDate = DateTime.Now.ToString("dd-MM-yyyy");
         
            var result = await connection.ExecuteAsync(
                "sp_editmember",
                new
                {
                    memNo = model.MemNo,
                    givenName = model.GivenName,
                    sureName = model.SureName,
                    phone = model.Phone,
                    email = model.Email,
                    niD = model.NiD,
                    bicNo = model.BiCNo,
                    passportNo = model.PassportNo,
                    nationality = model.Nationality,
                    gender = model.Gender,
                    father = model.Father,
                    mother = model.Mother,
                    address = model.Address,
                    idenDocu = model.IdenDocu,
                    photo = model.Photo,
                    updateDate = model.UpdateDate
                },
                commandType: CommandType.StoredProcedure);

            return "Member updated successfully";
        }

        public async Task<List<ArchiveMember>> GetArchivemember()
        {
            using var connection = new SqlConnection(_connectionString);

            var mem = await connection.QueryAsync<ArchiveMember>(
                "sp_acivememberList",
                commandType: CommandType.StoredProcedure
            );

            return mem.ToList();
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
                // check Base64 fields
                if (string.IsNullOrEmpty(model.IdenDocu))
                    return "No identification document provided";
                if (string.IsNullOrEmpty(model.Photo))
                    return "No photo provided";

                // ensure directories
                var uploadDirDoc = Path.Combine(Directory.GetCurrentDirectory(), "MemberDocu");
                var uploadDirPhoto = Path.Combine(Directory.GetCurrentDirectory(), "MemberPhoto");
                Directory.CreateDirectory(uploadDirDoc);
                Directory.CreateDirectory(uploadDirPhoto);

                // Save IdenDocu
                var idenBase64 = model.IdenDocu.Contains(",")
                    ? model.IdenDocu.Split(',').Last()
                    : model.IdenDocu;
                var idenBytes = Convert.FromBase64String(idenBase64);
                var docExt = ".pdf";
                if (model.IdenDocu.Contains("application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
                    docExt = ".docx";
                else if (model.IdenDocu.Contains("application/msword"))
                    docExt = ".doc";
                var docName = $"{Guid.NewGuid()}{docExt}";
                var docPathFull = Path.Combine(uploadDirDoc, docName);
                await File.WriteAllBytesAsync(docPathFull, idenBytes);

                // Save Photo
                var photoBase64 = model.Photo.Contains(",")
                    ? model.Photo.Split(',').Last()
                    : model.Photo;
                var photoBytes = Convert.FromBase64String(photoBase64);
                var photoExt = ".jpg";
                if (model.Photo.Contains("image/png")) photoExt = ".png";
                else if (model.Photo.Contains("image/jpeg")) photoExt = ".jpg";
                var photoName = $"{Guid.NewGuid()}{photoExt}";
                var photoPathFull = Path.Combine(uploadDirPhoto, photoName);
                await File.WriteAllBytesAsync(photoPathFull, photoBytes);

                // Set dates (DateTime হিসেবে)
                model.CreateDate = DateTime.Now.ToString("dd-MM-yyyy");
                model.UpdateDate = DateTime.Now.ToString("dd-MM-yyyy");

                string docDbPath = $"MemberDocu/{docName}";
                string photoDbPath = $"MemberPhoto/{photoName}";

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var parameters = new DynamicParameters();

                if (model.MemNo == 0)
                {
                    // Insert
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

                    await connection.ExecuteAsync("sp_InsertMember", parameters, commandType: CommandType.StoredProcedure);
                    return "Member saved successfully";
                }
                else
                {
                    if (model.Gender == 0)
                    {
                        model.Gender = 3;
                    }
                    // Update
                    parameters.Add("@memNo", model.MemNo);
                    parameters.Add("@givenName", model.GivenName);
                    parameters.Add("@sureName", model.SureName);
                    parameters.Add("@phone", model.Phone);
                    parameters.Add("@email", model.Email);
                    parameters.Add("@niD", model.NiD);
                    parameters.Add("@bicNo", model.BiCNo);
                    parameters.Add("@passportNo", model.PassportNo);
                    parameters.Add("@nationality", model.Nationality);
                    parameters.Add("@gender", model.Gender);
                    parameters.Add("@father", model.Father);
                    parameters.Add("@mother", model.Mother);
                    parameters.Add("@address", model.Address);
                    parameters.Add("@photo", photoDbPath);
                    parameters.Add("@idenDocu", docDbPath);
                    parameters.Add("@updateDate", model.UpdateDate);

                    await connection.ExecuteAsync("sp_editmember", parameters, commandType: CommandType.StoredProcedure);
                    return "Member updated successfully";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<List<MemberTransferLogs>> TransferLogsList()
        {
            using var connection = new SqlConnection(_connectionString);

            var mem = await connection.QueryAsync<MemberTransferLogs>(
                "sp_transferLogsHistory",
                commandType: CommandType.StoredProcedure
            );

            return mem.ToList();
        }

        public async Task<string> TransferMember(Members model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.IdenDocu))
                    return "No identification document provided";
                if (string.IsNullOrEmpty(model.Photo))
                    return "No photo provided";

                // ensure directories
                var uploadDirDoc = Path.Combine(Directory.GetCurrentDirectory(), "MemberDocu");
                var uploadDirPhoto = Path.Combine(Directory.GetCurrentDirectory(), "MemberPhoto");
                Directory.CreateDirectory(uploadDirDoc);
                Directory.CreateDirectory(uploadDirPhoto);

                // Save IdenDocu
                var idenBase64 = model.IdenDocu.Contains(",")
                    ? model.IdenDocu.Split(',').Last()
                    : model.IdenDocu;
                var idenBytes = Convert.FromBase64String(idenBase64);
                var docExt = ".pdf";
                if (model.IdenDocu.Contains("application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
                    docExt = ".docx";
                else if (model.IdenDocu.Contains("application/msword"))
                    docExt = ".doc";
                var docName = $"{Guid.NewGuid()}{docExt}";
                var docPathFull = Path.Combine(uploadDirDoc, docName);
                await File.WriteAllBytesAsync(docPathFull, idenBytes);

                // Save Photo
                var photoBase64 = model.Photo.Contains(",")
                    ? model.Photo.Split(',').Last()
                    : model.Photo;
                var photoBytes = Convert.FromBase64String(photoBase64);
                var photoExt = ".jpg";
                if (model.Photo.Contains("image/png")) photoExt = ".png";
                else if (model.Photo.Contains("image/jpeg")) photoExt = ".jpg";
                var photoName = $"{Guid.NewGuid()}{photoExt}";
                var photoPathFull = Path.Combine(uploadDirPhoto, photoName);
                await File.WriteAllBytesAsync(photoPathFull, photoBytes);

                // Set dates (DateTime হিসেবে)
                model.CreateDate = DateTime.Now.ToString("dd-MM-yyyy");
                model.UpdateDate = DateTime.Now.ToString("dd-MM-yyyy");

                string docDbPath = $"MemberDocu/{docName}";
                string photoDbPath = $"MemberPhoto/{photoName}";
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@memno", model.MemNo, DbType.Int32);
                parameters.Add("@givenname", model.GivenName, DbType.String);
                parameters.Add("@surename", model.SureName, DbType.String);
                parameters.Add("@phone", model.Phone, DbType.String);
                parameters.Add("@email", model.Email, DbType.String);
                parameters.Add("@nid", model.NiD, DbType.String);
                parameters.Add("@bicno", model.BiCNo, DbType.String);
                parameters.Add("@passportno", model.PassportNo, DbType.String);
                parameters.Add("@nationality", model.Nationality, DbType.String);
                parameters.Add("@gender", model.Gender, DbType.Int32);
                parameters.Add("@father", model.Father, DbType.String);
                parameters.Add("@mother", model.Mother, DbType.String);
                parameters.Add("@address", model.Address, DbType.String);
                parameters.Add("@idendocu", docDbPath, DbType.String);
                parameters.Add("@photo", photoDbPath, DbType.String);
                parameters.Add("@createdate", model.CreateDate, DbType.String);
                parameters.Add("@createby", model.CreateBy, DbType.String);
                parameters.Add("@updatedate", model.UpdateDate, DbType.String);

                await connection.ExecuteAsync(
                    "sp_membertrnsfer",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return "Member transferred successfully.";
            }
            catch (Exception ex)
            {
                // Optional: log error
                return $"Error: {ex.Message}";
            }
        }
 
    }
}
