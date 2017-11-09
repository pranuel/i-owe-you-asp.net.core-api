using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace I.Owe.You.Api.Model
{
    public class DebtsSummary : Entity
    {

        public int PartnerId { get; set; }
        public User Partner { get; set; }
        public int MeId { get; set; }
        public User Me { get; set; }
        public ICollection<Debt> Debts { get; set; }
        // this value is calculated by the debts list and won't be stored in the database:
        [NotMapped]
        public float DebtDifference { get; set; }
        // timestamp of the newest debt in the debts list (calculated by debts list, too):
        [NotMapped]
        public long LastDebtTimestamp { get; set; }

    }
}