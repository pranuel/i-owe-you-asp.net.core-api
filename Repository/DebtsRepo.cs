using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using I.Owe.You.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace I.Owe.You.Api.Repository
{

    public class DebtsRepo
    {

        private readonly ApiContext _context;
        private readonly DebtsSummariesRepo _debtsSummariesRepo;

        public DebtsRepo(ApiContext context, DebtsSummariesRepo debtsSummariesRepo)
        {
            _context = context;
            _debtsSummariesRepo = debtsSummariesRepo;
        }

        public async Task<Debt[]> GetAllDebtsAsync() {
            return await _context.Debts.ToArrayAsync();
        }

        public async Task AddDebtAsync(Debt debt) {
            await this._context.Debts.AddAsync(debt);
            await this._context.SaveChangesAsync();
            await _debtsSummariesRepo.UpdateSummariesAsync(debt.Creditor, debt.Debtor, debt.Amount, debt.Timestamp);
        }
    }
}