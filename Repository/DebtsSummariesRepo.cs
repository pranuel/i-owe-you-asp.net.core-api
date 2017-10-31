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

        public async Task<DebtsSummary> GetDebtsSummaryByIdAsync(int id)
        {
            return await _context.DebtsSummaries
                .Include(ds => ds.Me)
                .Include(ds => ds.Partner)
                .FirstOrDefaultAsync(ds => ds.Id == id);
        }

        public async Task<List<DebtsSummary>> GetAllDebtsSummariesForMeAsync(string mySub)
        {
            return await _context.DebtsSummaries
                .Include(ds => ds.Me)
                .Include(ds => ds.Partner)
                .Where(ds => ds.Me.Sub == mySub)
                .ToListAsync();
        }

        public async Task UpdateSummariesAsync(User creditor, User debtor, float amount, long debtTimestamp)
        {
            var creditorSummary = await this.GetDebtsSummaryByUserIdAsync(creditor.Id);
            if (creditorSummary == null)
            {
                creditorSummary = CreateDebtsSummary(creditor, debtor, amount, debtTimestamp);
            }
            else
            {
                creditorSummary.DebtDifference += amount;
                creditorSummary.LastDebtTimestamp = debtTimestamp;
            }
            await this.CreateOrUpdateDebtsSummaryByUserAsync(creditorSummary);

            var debtorSummary = await this.GetDebtsSummaryByUserIdAsync(debtor.Id);
            if (debtorSummary == null)
            {
                debtorSummary = CreateDebtsSummary(debtor, creditor, -amount, debtTimestamp);
            }
            else
            {
                debtorSummary.DebtDifference -= amount;
                debtorSummary.LastDebtTimestamp = debtTimestamp;
            }
            await this.CreateOrUpdateDebtsSummaryByUserAsync(debtorSummary);
        }

        private DebtsSummary CreateDebtsSummary(User me, User partner, float debtDifference, long lastDebtTimestamp)
        {
            return new DebtsSummary
            {
                PartnerId = partner.Id,
                MeId = me.Id,
                DebtDifference = debtDifference,
                LastDebtTimestamp = lastDebtTimestamp
            };
        }

        private async Task<DebtsSummary> GetDebtsSummaryByUserIdAsync(int userId)
        {
            return await this._context.DebtsSummaries.FirstOrDefaultAsync(ds => ds.MeId == userId);
        }

        private async Task CreateOrUpdateDebtsSummaryByUserAsync(DebtsSummary debtsSummaryByUser)
        {
            var existingDebtsSummary = await GetDebtsSummaryByUserIdAsync(debtsSummaryByUser.PartnerId);
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