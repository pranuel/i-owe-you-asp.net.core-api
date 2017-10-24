namespace I.Owe.You.Api.Model
{
    public class DebtsSummary : Entity
    {

        public int PartnerId { get; set; }
        public User Partner { get; set; }
        public float DebtDifference { get; set; }
        public long LastDebtTimestamp { get; set; }

    }
}