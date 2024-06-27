using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Linq; 

    namespace ProtectedDba.Controllers {
   
   //routes
   [ApiController]
   [Route("api/[controller]")]
     public class AnonimizeController : ControllerBase
     {
         // [HttpPost] //public IActionResult AnonimizeData([FromBody] JsonObject request data)//{ //JArray jsonArray = (JArray)requestData["data"];// JArray anonimizedArray = new JArray();//  }
        private readonly AppDbContext _context;
        public AnonimizeController(AppDbContext context)
        {
            _context = context;
        }
     }
    }
    [HttpPost("anonimize")]
    //POST request to receive JSON data
     public IActionResult AnonimizeData([FromBody] JObject requestData)
     {
         JArray jsonArray = (JArray)requestData["data"]; //extract an empty json array
         JArray anonimizedArray = new JArray(); //Creates a new empty JSON array

         //extraction, anonymization and hashing of data
         foreach(var item in jsonArray) {
            
            string originalName = item["name"].ToString();
            string gender = item["gender"].ToString();
            string originalCPF = item["Cpf"].ToString();

         }
     }

     