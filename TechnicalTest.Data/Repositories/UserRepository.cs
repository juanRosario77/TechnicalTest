using Microsoft.EntityFrameworkCore;
using TechnicalTest.Data.Interfaces;
using TechnicalTest.Data.Models;

namespace TechnicalTest.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TechnicalTestContext _context;

        public UserRepository(TechnicalTestContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByEmailandPasswordAsync(string? email, string? password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email
                && u.Password == password);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }

}
