using System.Threading.Tasks;
using I.Owe.You.Api.Model;
using I.Owe.You.Api.Repository;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;

namespace I.Owe.You.Api.Tests
{

    public class DebtsSummariesRepoTests
    {

        [Fact]
        public async Task Get_debts_summary()
        {
            var options = await SetupAndReturnContextOptionsAsync("Get_debts_summary");

            using (var context = new ApiContext(options))
            {
                var sut = new DebtsSummariesRepo(context);
                var debtsSummaries = await sut.GetAllDebtsSummariesForMeAsync("facebook|1080925881970593");
                debtsSummaries.Count.Should().Be(1);
                debtsSummaries.Exists(ds => ds.DebtDifference == -90).Should().BeTrue();
            }
        }

        [Fact]
        public async Task Should_return_no_debts()
        {
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase("Should_return_no_debts")
                .Options;
            using (var context = new ApiContext(options))
            {
                var sut = new DebtsSummariesRepo(context);
                var debtsSummaries = await sut.GetAllDebtsSummariesForMeAsync("facebook|1080925881970593");
                debtsSummaries.Count.Should().Be(0);
            }
        }

        private async Task<DbContextOptions<ApiContext>> SetupAndReturnContextOptionsAsync(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            using (var context = new ApiContext(options))
            {
                var usersRepo = new UsersRepo(context);
                var testUser1 = new User
                {
                    Id = 1,
                    Name = "Luke Skywalker",
                    Sub = "facebook|1080925881970593"
                };
                await usersRepo.AddUserAsync(testUser1);

                var testUser2 = new User
                {
                    Id = 2,
                    Name = "Darth Vader",
                    Sub = "test sub 2"
                };
                await usersRepo.AddUserAsync(testUser2);

                var debtsSummariesRepo = new DebtsSummariesRepo(context);
                var debtsRepo = new DebtsRepo(context, debtsSummariesRepo);
                var testDebt1 = new Debt
                {
                    Debtor = testUser1,
                    DebtorId = testUser1.Id,
                    Creditor = testUser2,
                    CreditorId = testUser2.Id,
                    Amount = 100,
                    Reason = "build death star"
                };
                await debtsRepo.AddDebtAsync(testDebt1);

                var testDebt2 = new Debt
                {
                    Debtor = testUser2,
                    DebtorId = testUser2.Id,
                    Creditor = testUser1,
                    CreditorId = testUser1.Id,
                    Amount = 10,
                    Reason = "buy sweets"
                };
                await debtsRepo.AddDebtAsync(testDebt2);
            }

            return options;
        }

    }

}