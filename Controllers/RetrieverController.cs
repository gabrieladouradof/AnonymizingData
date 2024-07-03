using Microsoft.Extensions.Configuration; // for configuration
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ProtectedDba.Models;
using System.Linq


namespace ProtectedDba.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class RetrieverController : ControllerBase
   {
      private readonly AppDbContext _context;
      private readonly string _hashKey; //not nullable

       // O parâmetro IConfiguration é utilizado para ler as configurações
      public RetrieverController(AppDbContext context, IConfiguration configuration)
      {
         _context = context; 
         _hashKey = configuration.GetValue<string>("HashKey")?? throw new ArgumentNullException(nameof(configuration["HashKey"]), "HashKey configuration value is missing.");
      }

      [HttpPost("retrieve")]
      public IActionResult RetrieveData([FromBody] JObject requestData) 
      {
         if (requestData["data"] is not JArray jsonArray) {
            return BadRequest("Invalid data format. 'data' should be an array");
         }

         JArray retrievedArray = new JArray();

         foreach(var item in jsonArray) 
         {
            string hashedCpf = item["cpf"]?.ToString() ?? string.Empty;

            if (string.IsNullOrEmpty(hashedCpf))
                {
                    continue;
                }

                var userAnonymized = _context.UserAnonymized
                    .FirstOrDefault(u => HashCpf(u.Cpf, _hashKey) == hashedCpf);

                if (userAnonymized != null)
                {
                    JObject retrievedItem = new JObject
                    {
                        { "name", userAnonymized.Name },
                        { "cpf", userAnonymized.Cpf },
                        { "gender", userAnonymized.Gender }
                    };

                    retrievedArray.Add(retrievedItem);
                }
            }

            return Ok(retrievedArray);
        }

      private string HashCpf(string originalCPF, string hashKey)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string combinedInput = originalCPF + hashKey;
                byte[] hashedBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(combinedInput));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

         }
      }
   }
} 
