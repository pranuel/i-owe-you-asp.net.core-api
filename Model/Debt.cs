namespace I.Owe.You.Api.Model
{
    public class Debt : Entity
    {

        public User Debtor { get; set; }
        public User Creditor { get; set; }
        public float Amount { get; set; }
        public int Timestamp { get; set; }
        public string Reason { get; set; }

    }
}
