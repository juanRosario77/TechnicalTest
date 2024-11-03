using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TechnicalTest.Core.DTOs;
using TechnicalTest.Core.Interfaces;
using TechnicalTest.Data.Models;

namespace TechnicalTest.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly AuthenticationConfig _authentication;
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public AccountService(IOptions<AuthenticationConfig> authentication)
        {
            _authentication = authentication.Value;

            if (string.IsNullOrWhiteSpace(_authentication.SecretKey))
            {
                throw new ArgumentNullException("Settings: " + nameof(_authentication.SecretKey), ", cannot be empty or white space");
            }
            if (_authentication.SecretKey.Trim().Length < 32)
            {
                throw new ArgumentNullException("Settings: " + nameof(_authentication.SecretKey), ", minimun length is 32 characters");
            }
            else if (_authentication.Duration == 0)
            {
                throw new ArgumentNullException("Settings: " + nameof(_authentication.Duration) + ", can be zero");
            }

            using var sha256 = SHA256.Create();
            _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(_authentication.SecretKey));
            _iv = _key[..16];
        }

        public string GenerateJwtToken(User user)
        {
            if (user.IsActive)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_authentication.SecretKey ?? string.Empty);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                    Expires = DateTime.UtcNow.AddHours(_authentication.Duration),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature
                    )
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            else
            {
                throw new ValidationException("User is not Active");
            }
        }

        public string EncryptPassword(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public string DecryptPassword(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }
    }
}
