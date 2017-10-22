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

        public async Task<List<DebtsSummary>> GetAllDebtsSummariesForMeAsync(string mySub)
        {
            return await _context.DebtsSummaries
                .Include(ds => ds.User)
                .Where(ds => ds.User.Sub == mySub)
                .ToListAsync();
        }

        public async Task UpdateSummariesAsync(User creditor, User debtor, float amount, int debtTimestamp)
        {
            var creditorSummary = await this.GetDebtsSummaryByUserIdAsync(creditor.Id);
            if (creditorSummary == null)
            {
                creditorSummary = CreateDebtsSummary(creditor, amount, debtTimestamp);
            }
            else
            {
                creditorSummary.DebtDifference += amount;
            }
            await this.CreateOrUpdateDebtsSummaryByUserAsync(creditorSummary);

            var debtorSummary = await this.GetDebtsSummaryByUserIdAsync(debtor.Id);
            if (debtorSummary == null)
            {
                debtorSummary = CreateDebtsSummary(debtor, -amount, debtTimestamp);
            }
            else
            {
                debtorSummary.DebtDifference -= amount;
            }
            await this.CreateOrUpdateDebtsSummaryByUserAsync(debtorSummary);
        }

        private DebtsSummary CreateDebtsSummary(User user, float debtDifference, int lastDebtTimestamp)
        {
            return new DebtsSummary
            {
                User = user,
                UserId = user.Id,
                DebtDifference = debtDifference,
                LastDebtTimestamp = lastDebtTimestamp
            };
        }

        private async Task<DebtsSummary> GetDebtsSummaryByUserIdAsync(int userId)
        {
            return await this._context.DebtsSummaries.FirstOrDefaultAsync(dsbu => dsbu.UserId == userId);
        }

        private async Task CreateOrUpdateDebtsSummaryByUserAsync(DebtsSummary debtsSummaryByUser)
        {
            var existingDebtsSummary = await GetDebtsSummaryByUserIdAsync(debtsSummaryByUser.UserId);
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