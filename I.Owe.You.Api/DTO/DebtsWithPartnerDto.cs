using I.Owe.You.Api.Model;

namespace I.Owe.You.Api.DTO
{
    public class DebtsWithPartner
    {

        public int PartnerId { get; set; }
        public Debt[] Debts { get; set; }

    }
}
