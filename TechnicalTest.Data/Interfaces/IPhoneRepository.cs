using TechnicalTest.Data.Models;

namespace TechnicalTest.Data.Interfaces
{
    public interface IPhoneRepository
    {
        Task<Phone> GetPhoneByIdAsync(Guid id);
        Task<IEnumerable<Phone>> GetPhonesByUserIdAsync(Guid userId);
        Task AddPhoneAsync(Phone phone);
        Task UpdatePhoneAsync(Phone phone);
        Task DeletePhoneAsync(Guid id);
    }
}
