using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using I.Owe.You.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace I.Owe.You.Api.Repository
{

    public class UsersRepo
    {

        private readonly ApiContext _context;

        public UsersRepo(ApiContext context)
        {
            _context = context;
        }

        public async Task<User[]> GetAllUsers()
        {
            return await _context.Users.ToArrayAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddMeAsync(User me, string sub)
        {
            // set / override sub of me (don't trust the client ;) )
            me.Sub = sub;
            await _context.Users.AddAsync(me);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.Where(u => u.Id == id).SingleOrDefaultAsync();
        }

        public async Task<User> GetUserBySubAsync(string sub)
        {
            // docu: https://docs.microsoft.com/en-us/ef/core/querying/async
            return await _context.Users.Where(u => u.Sub == sub).SingleOrDefaultAsync();
        }

        public async Task<User> GetUserByName(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
        }
    }
}