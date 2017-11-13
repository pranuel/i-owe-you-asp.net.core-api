using System.Collections.Generic;

namespace I.Owe.You.Api.Model
{
    public class Debt : Entity
    {

        public int DebtorId { get; set; }
        public User Debtor { get; set; }
        public int CreditorId { get; set; }
        public User Creditor { get; set; }
        public float Amount { get; set; }
        public long Timestamp { get; set; }
        public string Reason { get; set; }
        public bool IsRepaid { get; set; }
        public string DebtsGroupId { get; set; }
        public DebtsGroup DebtsGroup { get; set; }

    }
}
