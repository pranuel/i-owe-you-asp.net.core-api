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

        public async Task<Debt[]> GetAllDebtsAsync()
        {
            return await _context.Debts.ToArrayAsync();
        }

        public async Task<Debt[]> GetAllDebtsForPartner(int partnerId)
        {
            return await _context.Debts
                .Where(d => d.CreditorId == partnerId || d.DebtorId == partnerId)
                .Include(d => d.Creditor)
                .Include(d => d.Debtor)
                .ToArrayAsync();
        }

        public async Task AddDebtAsync(Debt debt)
        {
            // save some values, then...
            var debtor = debt.Debtor;
            var creditor = debt.Creditor;
            // ... reset some values:
            debt.Id = 0;
            debt.Creditor = null;
            debt.Debtor = null;
            await this._context.Debts.AddAsync(debt);
            await this._context.SaveChangesAsync();
            await _debtsSummariesRepo.UpdateSummariesAsync(debt);
        }
    }
}