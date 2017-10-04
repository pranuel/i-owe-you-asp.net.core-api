namespace I.Owe.You.Api.Model
{
    public class DebtsSummaryByUser
    {

        public User User { get; set; }
        public float DebtDifference { get; set; }

    }
}