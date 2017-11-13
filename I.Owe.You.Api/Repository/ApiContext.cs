using I.Owe.You.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace I.Owe.You.Api.Repository
{

    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Debt> Debts { get; set; }

        public DbSet<DebtsGroup> DebtsGroups { get; set; }

    }

}