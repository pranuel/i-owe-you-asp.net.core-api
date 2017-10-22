namespace I.Owe.You.Api.Model
{
    public class Debt : Entity
    {

        public int DebtorId { get; set; }
        public User Debtor { get; set; }
        public int CreditorId { get; set; }
        public User Creditor { get; set; }
        public float Amount { get; set; }
        public int Timestamp { get; set; }
        public string Reason { get; set; }

    }
}
