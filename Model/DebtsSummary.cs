namespace I.Owe.You.Api.Model
{
    public class DebtsSummary : Entity
    {

        public int UserId { get; set; }
        public User User { get; set; }
        public float DebtDifference { get; set; }
        public int LastDebtTimestamp { get; set; }

    }
}