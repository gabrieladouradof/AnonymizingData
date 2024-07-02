using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq; 
using ProtectedDba.Models;
  
  namespace ProtectedDba.Controllers {
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
       } }