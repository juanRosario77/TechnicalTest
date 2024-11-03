using TechnicalTest.Data.Models;

namespace TechnicalTest.Core.Interfaces
{
    public interface IAccountService
    {
        string GenerateJwtToken(User user);
        string EncryptPassword(string plainText);
        string DecryptPassword(string cipherText);
    }
}
