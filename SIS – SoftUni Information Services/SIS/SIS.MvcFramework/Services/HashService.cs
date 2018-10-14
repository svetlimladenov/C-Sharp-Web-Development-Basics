using System;
using System.Security.Cryptography;
using System.Text;
using SIS.MvcFramework.Logger;

namespace SIS.MvcFramework.Services
{
    public class HashService : IHashService
    {
        private readonly ILogger logger;

        public HashService(ILogger logger)
        {
            this.logger = logger;
        }

        public string StrongHash(string stringToHash)
        {
            var result = stringToHash;
            for (int i = 0; i < 3; i++)
            {
                result = Hash(result);
            }

            return result;
        }

        public string Hash(string stringToHash)
        {
            stringToHash = stringToHash + "myAppSalt12312341234#";
            // SHA256 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
                // Get the hashed string.  
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                // Print the string.   
                this.logger.Log(hash);
                return hash;
            }
        }


    }
}
