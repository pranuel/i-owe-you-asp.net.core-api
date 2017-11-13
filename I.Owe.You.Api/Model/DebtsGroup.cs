using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace I.Owe.You.Api.Model
{
    public class DebtsGroup : Entity
    {
        public DebtsGroup()
        {
            this.Debts = new HashSet<Debt>();
        }

        public int User1Id { get; set; }
        public User User1 { get; set; }
        public int User2Id { get; set; }
        public User User2 { get; set; }
        public ICollection<Debt> Debts { get; set; }
        // this value is calculated by the debts list and won't be stored in the database:
        [NotMapped]
        public float DebtDifference { get; set; }
        // timestamp of the newest debt in the debts list (calculated by debts list, too):
        [NotMapped]
        public long LastDebtTimestamp { get; set; }

    }
}