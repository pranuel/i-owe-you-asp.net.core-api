using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using I.Owe.You.Api.Model;

namespace I.Owe.You.Api.Repository
{

    public class DebtsRepo
    {

        private readonly ApiContext context;

        public DebtsRepo(ApiContext context)
        {
            this.context = context;
        }

        public async Task AddDebtAsync(Debt debt) {
            await this.context.Debts.AddAsync(debt);
            await this.context.SaveChangesAsync();
        }
    }
}