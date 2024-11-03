using Microsoft.EntityFrameworkCore;
using TechnicalTest.Data.Interfaces;
using TechnicalTest.Data.Models;

namespace TechnicalTest.Data.Repositories
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly TechnicalTestContext _context;

        public PhoneRepository(TechnicalTestContext context)
        {
            _context = context;
        }

        public async Task<Phone> GetPhoneByIdAsync(Guid id)
        {
            return await _context.Phones.FindAsync(id);
        }

        public async Task<IEnumerable<Phone>> GetPhonesByUserIdAsync(Guid userId)
        {
            return await _context.Phones.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task AddPhoneAsync(Phone phone)
        {
            await _context.Phones.AddAsync(phone);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePhoneAsync(Phone phone)
        {
            _context.Phones.Update(phone);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePhoneAsync(Guid id)
        {
            var phone = await GetPhoneByIdAsync(id);
            if (phone != null)
            {
                _context.Phones.Remove(phone);
                await _context.SaveChangesAsync();
            }
        }
    }

}
