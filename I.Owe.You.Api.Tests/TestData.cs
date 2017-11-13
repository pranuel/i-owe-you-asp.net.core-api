using I.Owe.You.Api.Model;

namespace I.Owe.You.Api.Tests
{

    public class TestData
    {
        public static readonly User User1 = new User
        {
            Id = 1,
            Name = "Luke Skywalker",
            Sub = "facebook|1080925881970593"
        };

        public static readonly User User2 = new User
        {
            Id = 2,
            Name = "Darth Vader",
            Sub = "test sub 2"
        };

        public static readonly Debt Debt1 = new Debt
        {
            Debtor = User1,
            DebtorId = User1.Id,
            Creditor = User2,
            CreditorId = User2.Id,
            Amount = 100,
            Reason = "build death star"
        };

        public static readonly Debt Debt2 = new Debt
        {
            Debtor = User2,
            DebtorId = User2.Id,
            Creditor = User1,
            CreditorId = User1.Id,
            Amount = 10,
            Reason = "buy sweets"
        };

        public static readonly Debt Debt3 = new Debt
        {
            Debtor = User2,
            DebtorId = User2.Id,
            Creditor = User1,
            CreditorId = User1.Id,
            Amount = 5,
            Reason = "buy milk"
        };

    }

}