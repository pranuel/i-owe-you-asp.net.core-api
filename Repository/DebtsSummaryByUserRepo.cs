using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using I.Owe.You.Api.Model;

namespace I.Owe.You.Api.Repository
{

    public class DebtsSummaryByUserRepo
    {

        private readonly ApiContext context;

        public DebtsSummaryByUserRepo(ApiContext context)
        {
            this.context = context;
        }

        public async void UpdateSummaryBetweenUsersAsync(User creditor, User debtor, float amount)
        {
            var creditorSummary = await this.GetDebtsSummaryByUserIdAsync(creditor.Id);
            creditorSummary.DebtDifference += amount;
            this.UpdateDebtsSummaryByUserAsync(creditorSummary);

            var debtorSummary = await this.GetDebtsSummaryByUserIdAsync(debtor.Id);
            debtorSummary.DebtDifference -= amount;
            this.UpdateDebtsSummaryByUserAsync(debtorSummary);
        }

        private async Task<DebtsSummaryByUser> GetDebtsSummaryByUserIdAsync(int userId)
        {
            // try to get the dsbu from the context:
            var valueFromContext = this.context.DebtsSummariesByUser.FirstOrDefault(dsbu => dsbu.UserId == userId);
            // if it does not exist in the context create it with the default values:
            if (valueFromContext == null)
            {
                var defaultValue = new DebtsSummaryByUser
                {
                    UserId = userId,
                    DebtDifference = 0
                };
                var newValueFromContext = await this.context.AddAsync(defaultValue);
                valueFromContext = newValueFromContext.Entity;
            }
            // then return the existing or added value from the context:
            return valueFromContext;
        }

        public async void UpdateDebtsSummaryByUserAsync(DebtsSummaryByUser debtsSummaryByUser)
        {
            this.context.DebtsSummariesByUser.Update(debtsSummaryByUser);
            await this.context.SaveChangesAsync();
        }


    }

}