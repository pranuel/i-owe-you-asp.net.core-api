using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using I.Owe.You.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace I.Owe.You.Api.Repository
{

    public class DebtsGroupRepo
    {

        private readonly ApiContext _context;

        public DebtsGroupRepo(ApiContext context)
        {
            _context = context;
        }

        public async Task<DebtsGroup> GetDebtsSummaryByIdAsync(int id, string mySub)
        {
            return await IncludeRequiredFields(_context.DebtsGroups)
                .Select(ds => IncludeUnmappedFields(ds, mySub))
                .FirstOrDefaultAsync(ds => ds.Id == id);
        }

        public async Task<List<DebtsGroup>> GetAllDebtsSummariesForMeAsync(string mySub)
        {
            return await IncludeRequiredFields(_context.DebtsGroups)
                .Where(ds => ds.User2.Sub == mySub)
                .Select(ds => IncludeUnmappedFields(ds, mySub))
                .ToListAsync();
        }

        private IQueryable<DebtsGroup> IncludeRequiredFields(DbSet<DebtsGroup> debtsSummaries)
        {
            return debtsSummaries
                .Include(ds => ds.Debts)
                .Include(ds => ds.User2)
                .Include(ds => ds.User1);
        }

        private DebtsGroup IncludeUnmappedFields(DebtsGroup debtsSummary, string mySub)
        {
            var debtDifference = debtsSummary.Debts
                    .Where(d => !d.IsRepaid)
                    .Select(d => (d.Creditor.Sub == mySub) ? d.Amount : (-1 * d.Amount))
                    .Sum(amount => amount);
            var lastTimestamp = debtsSummary.Debts
            .Where(d => !d.IsRepaid)
            .Select(d => d.Timestamp)
            .OrderByDescending(t => t)
            .FirstOrDefault();
            debtsSummary.DebtDifference = debtDifference;
            debtsSummary.LastDebtTimestamp = lastTimestamp;
            return debtsSummary;
        }

        public async Task UpdateSummariesAsync(Debt debt)
        {
            var creditorId = debt.CreditorId;
            var debtorId = debt.DebtorId;
            var debtsGroup = await GetDebtsSummaryByUserIdAsync(creditorId, debtorId);
            if (debtsGroup == null)
            {
                debtsGroup = new DebtsGroup
                {
                    User1Id = creditorId,
                    User2Id = debtorId,
                };
                debtsGroup.Debts.Add(debt);
                await _context.DebtsGroups.AddAsync(debtsGroup);
            }
            else
            {
                debtsGroup.Debts.Add(debt);
            }
            await this._context.SaveChangesAsync();
        }

        private async Task<DebtsGroup> GetDebtsSummaryByUserIdAsync(int userId1, int userId2)
        {
            return await this._context.DebtsGroups
                .FirstOrDefaultAsync(ds =>
                    (ds.User1Id == userId1 && ds.User2Id == userId2) ||
                    (ds.User2Id == userId1 && ds.User1Id == userId2));
        }


    }

}