using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using I.Owe.You.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace I.Owe.You.Api.Repository
{

    public class UsersRepo
    {

        private readonly ApiContext context;

        public UsersRepo(ApiContext context)
        {
            this.context = context;
        }

        public async Task AddUserAsync(User user) {
            await this.context.Users.AddAsync(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<User> GetUserBySubAsync(string sub) {
            var foo = await this.context.Users.ToListAsync();
            // docu: https://docs.microsoft.com/en-us/ef/core/querying/async
            return await this.context.Users.Where(u => u.Sub == sub).SingleOrDefaultAsync();
        }
    }
}