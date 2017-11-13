using System.Linq;
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
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase("Get_debts_summary")
                .Options;

            using (var context = new ApiContext(options))
            {
                var usersRepo = new UsersRepo(context);
                await usersRepo.AddUserAsync(TestData.User1);
                await usersRepo.AddUserAsync(TestData.User2);

                var debtsSummariesRepo = new DebtsGroupRepo(context);
                var debtsRepo = new DebtsRepo(context, debtsSummariesRepo);
                
                await debtsRepo.AddDebtAsync(TestData.Debt1);                
                await debtsRepo.AddDebtAsync(TestData.Debt2);                
                await debtsRepo.AddDebtAsync(TestData.Debt3);

                var allDebts = await debtsRepo.GetAllDebtsAsync();
                allDebts.Length.Should().Be(3);
            }

            using (var context = new ApiContext(options))
            {
                var sut = new DebtsGroupRepo(context);
                var debtsSummaries = await sut.GetAllDebtsSummariesForMeAsync(TestData.User1.Sub);
                debtsSummaries.Count.Should().Be(1);
                var foo = debtsSummaries.FirstOrDefault();
                debtsSummaries.Exists(ds => ds.DebtDifference == -85).Should().BeTrue();
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
                var sut = new DebtsGroupRepo(context);
                var debtsSummaries = await sut.GetAllDebtsSummariesForMeAsync("facebook|1080925881970593");
                debtsSummaries.Count.Should().Be(0);
            }
        }

    }

}