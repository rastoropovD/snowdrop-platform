using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Snowdrop.BL.Tests.Unit.Services.Users
{
    public class HashHelper
    {
        //Microsoft.AspNetCore.Cryptography.KeyDerivation
        public static string Create(string value, string salt)  
        {  
            byte[] valueBytes = KeyDerivation.Pbkdf2(  
                password: value,  
                salt: Encoding.UTF8.GetBytes(salt),  
                prf: KeyDerivationPrf.HMACSHA512,  
                iterationCount: 10000,  
                numBytesRequested: 256 / 8);  

            return Convert.ToBase64String(valueBytes);  
        }
        
        public static bool Validate(string value, string salt, string hash)  
            => Create(value, salt) == hash;  
    }
}