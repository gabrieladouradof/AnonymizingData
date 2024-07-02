using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq; 
using ProtectedDba.Models;

namespace ProtectedDba.Controllers {
   
   //routes
   [ApiController]
   [Route("api/[controller]")]
     public class AnonimizeController : ControllerBase
     {
        private readonly AppDbContext _context;
        public AnonimizeController(AppDbContext context)
        {
            _context = context;
        }
     
    
    //POST request to receive JSON data
    [HttpPost("anonimize")]
     public IActionResult AnonimizeData([FromBody] JObject requestData)
     {
         if (requestData["data"] is not JArray jsonArray)
         { return BadRequest("Invalid data format. 'data' should be an array");  }

         //JArray jsonArray = (JArray)requestData["data"]; //extract an empty json array
         JArray anonimizedArray = new JArray(); //Creates a new empty JSON array

         //extraction, anonymization and hashing of data
         foreach(var item in jsonArray) 
         {  
            //ToString() ?? string.Empty; -  to ensure strings are not null.
            string originalName = item["Name"]?.ToString() ?? string.Empty;
            string gender = item["Gender"]?.ToString() ?? string.Empty;
            string originalCPF = item["Cpf"]?.ToString() ?? string.Empty;
            string anonimizedName = AnonimizeName(originalName, gender, originalCPF);
            string hashedCpf = HashCPF(originalCPF);

         //assigning new instances
            var userAnonymized = new UserAnonymized
            {
               Name = anonimizedName,
               Cpf = hashedCpf,
               Gender = gender,
               HashKey = hashedCpf
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

      private string AnonimizeName (string originalName, string gender, string originalCPF)
      {
         return "Name anonimized";
      }

//responsible for generating a SHA-256 hash of the original CPF, 
//converting it into a hexadecimal string. This ensures that the CPF is securely anonymized.
      private string HashCPF (string originalCPF)
      {
         using (SHA256 sha256Hash = SHA256.Create())
         {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(originalCPF));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++) 
            {
               builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
         }
      }
     } } 
 
