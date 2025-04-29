using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace WebApplication1
{
    public class Class
    {
        private string _connectionString { get; set; } = "";
        public IActionResult GetUser(string userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = $"SELECT * FROM Users WHERE Id = '{userId}'"; // 👈 injection risk
                using (var command = new SqlCommand(query, connection))
                {
                    var reader = command.ExecuteReader();
                    // ...
                }
            }
            return Ok();
        }
        [AllowAnonymous]
        public IActionResult AdminOnly()
        {
            // Logic here assumes user is authenticated, but they aren't forced to be
            return Ok("You are an admin");
        }

        private IActionResult Ok()
        {
            throw new NotImplementedException();
        }

        public IActionResult LoadType(string className)
        {
            var type = Type.GetType(className); // 👈 attacker controls className
            var instance = Activator.CreateInstance(type);
            return Ok(instance);
        }

        private IActionResult Ok(object? instance)
        {
            throw new NotImplementedException();
        }
    }

}




public class Config
{
public string JwtSecret = "super_secret_key_123"; // 👈 hardcoded secret
}