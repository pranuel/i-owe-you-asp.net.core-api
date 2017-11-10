using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using I.Owe.You.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace I.Owe.You.Api.Repository
{

    public class DebtsSummariesRepo
    {

        private readonly ApiContext _context;

        public DebtsSummariesRepo(ApiContext context)
        {
            _context = context;
        }

        public async Task<DebtsSummary> GetDebtsSummaryByIdAsync(int id, string mySub)
        {
            return await IncludeRequiredFields(_context.DebtsSummaries)
                .Select(ds => IncludeUnmappedFields(ds, mySub))
                .FirstOrDefaultAsync(ds => ds.Id == id);
        }

        public async Task<List<DebtsSummary>> GetAllDebtsSummariesForMeAsync(string mySub)
        {
            return await IncludeRequiredFields(_context.DebtsSummaries)
                .Where(ds => ds.Me.Sub == mySub)
                .Select(ds => IncludeUnmappedFields(ds, mySub))
                .ToListAsync();
        }

        private IQueryable<DebtsSummary> IncludeRequiredFields(DbSet<DebtsSummary> debtsSummaries)
        {
            return debtsSummaries
                .Include(ds => ds.Debts)
                .Include(ds => ds.Me)
                .Include(ds => ds.Partner);
        }

        private DebtsSummary IncludeUnmappedFields(DebtsSummary debtsSummary, string mySub)
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
            await CreateOrUpdateDebtsSummaryForPartnerAndMeAsync(creditorId, debtorId, debt);
            await CreateOrUpdateDebtsSummaryForPartnerAndMeAsync(debtorId, creditorId, debt);
        }

        private async Task CreateOrUpdateDebtsSummaryForPartnerAndMeAsync(int meId, int partnerId, Debt debt)
        {

            var myDebtSummary = await GetDebtsSummaryByUserIdAsync(meId);
            if (myDebtSummary == null)
            {
                myDebtSummary = new DebtsSummary
                {
                    PartnerId = partnerId,
                    MeId = meId,
                    Debts = new List<Debt>()
                };
            }
            myDebtSummary.Debts.Add(debt);
            await CreateOrUpdateDebtsSummaryByUserAsync(myDebtSummary);
        }

        private async Task<DebtsSummary> GetDebtsSummaryByUserIdAsync(int userId)
        {
            return await this._context.DebtsSummaries.FirstOrDefaultAsync(ds => ds.MeId == userId);
        }

        private async Task CreateOrUpdateDebtsSummaryByUserAsync(DebtsSummary debtsSummaryByUser)
        {
            var existingDebtsSummary = await GetDebtsSummaryByUserIdAsync(debtsSummaryByUser.MeId);
            // reset partner object:
            debtsSummaryByUser.Partner = null;
            if (existingDebtsSummary == null)
            {
                await _context.DebtsSummaries.AddAsync(debtsSummaryByUser);
            }
            else
            {
                this._context.DebtsSummaries.Update(debtsSummaryByUser);
            }
            await this._context.SaveChangesAsync();
        }


    }

}