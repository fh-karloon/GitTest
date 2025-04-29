using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeQLDemoController : ControllerBase
    {
        private readonly string _connectionString = "Server=.;Database=TestDb;Trusted_Connection=True;";

        // ❌ SQL Injection
        [HttpGet("sql-injection")]
        public IActionResult GetUser(string userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = $"SELECT * FROM Users WHERE Id = '{userId}'"; // vulnerable!
                using (var command = new SqlCommand(query, connection))
                {
                    var reader = command.ExecuteReader();
                }
            }
            return Ok("Query executed");
        }

        // ❌ Insecure Deserialization
        //[HttpPost("deserialize")]
        //public IActionResult DeserializeData(string base64Payload)
        //{
        //    var bytes = Convert.FromBase64String(base64Payload);
        //    var formatter = new BinaryFormatter();  // insecure!
        //    var obj = formatter.Deserialize(new MemoryStream(bytes));
        //    return Ok("Deserialization complete");
        //}

        // ❌ Hardcoded Secret
        [HttpGet("hardcoded-secret")]
        public IActionResult GetSecret()
        {
            string secretKey = "super_secret_key_123";  // don't do this!
            return Ok(secretKey);
        }

        // ❌ Unsafe Reflection
        [HttpGet("unsafe-reflection")]
        public IActionResult LoadType(string className)
        {
            var type = Type.GetType(className); // unsafe
            var instance = Activator.CreateInstance(type);
            return Ok(instance.ToString());
        }

        // ❌ Missing Input Validation / Path Traversal
        [HttpPost("upload-file")]
        public IActionResult UploadFile(string filename)
        {
            var path = Path.Combine("uploads", filename); // no validation!
            System.IO.File.WriteAllText(path, "test data");
            return Ok("File written");
        }

        // ❌ Anonymous access on sensitive route
        [AllowAnonymous]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok("You are an admin — but no auth was required 😱");
        }
    }
}

