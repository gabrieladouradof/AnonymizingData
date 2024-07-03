using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq; 
using ProtectedDba.Models;
using ProtectedDba.Services;

namespace ProtectedDba.Controllers {
   
   //routes
   [ApiController]
   [Route("api/[controller]")]
     public class AnonimizeController : ControllerBase
     {
        private readonly AppDbContext _context;
        private readonly OpenAiService _openAiService;
        private readonly string _hashKey;
        public AnonimizeController(AppDbContext context, OpenAiService openAiService, string HashKey)
        {
            _context = context;
            _openAiService = openAiService;
            _hashKey = HashKey;
        }
         
    //POST request to receive JSON data
    [HttpPost("anonimize")]
     public async Task<IActionResult> AnonimizeData([FromBody] JObject requestData)
     {
         if (requestData["data"] is not JArray jsonArray)
         { return BadRequest("Invalid data format. 'data' should be an array");  }

         JArray anonimizedArray = new JArray(); //Creates a new empty JSON array

         //extraction, anonymization and hashing of data
         foreach(var item in jsonArray) 
         {  
            //ToString() ?? string.Empty; -  to ensure strings are not null.
            string originalName = item["Name"]?.ToString() ?? string.Empty;
            string gender = item["Gender"]?.ToString() ?? string.Empty;
            string originalCPF = item["Cpf"]?.ToString() ?? string.Empty;
         
         if (string.IsNullOrEmpty(originalName) || string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(originalCPF))
               {continue;}

         string anonimizedName = await _openAiService.GenerateAnonimizedName(originalName, gender);
         string hashedCpf = HashCPF(originalCPF, _hashKey);

         //assigning new instances
         var userAnonymized = new UserAnonymized
         {
            Name = anonimizedName,
            Cpf = hashedCpf,
            Gender = gender,
            HashKey = _hashKey
         };            

         _context.UserAnonymized.Add(userAnonymized);
         _context.SaveChanges();
            
            //new object Json for the anonimized dates
         JObject anonimizedItem = new JObject
         {
            {"name", anonimizedName},
            { "cpf", hashedCpf},
            { "gender", gender }
         };

            anonimizedArray.Add(anonimizedItem); //add for the anonimizedarray
         }
         return Ok(anonimizedArray); //return hhtp200
     }


//responsible for generating a SHA-256 hash of the original CPF, 
//converting it into a hexadecimal string. This ensures that the CPF is securely anonymized.
      private string HashCPF (string originalCPF, string hashKey)
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
 
