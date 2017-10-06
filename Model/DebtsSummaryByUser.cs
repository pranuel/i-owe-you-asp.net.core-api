namespace I.Owe.You.Api.Model
{
    public class DebtsSummaryByUser : Entity
    {

        public int UserId { get; set; }
        public float DebtDifference { get; set; }

    }
}