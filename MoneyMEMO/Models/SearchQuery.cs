using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMEMO.Models
{
    public class SearchQuery
    {
        public DateTime? Date { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public decimal? Amount { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }

        public bool UseAmountRange { get; set; }
        public bool UseDateRange { get; set; }

        public bool IncludeIncome { get; set; }
        public bool IncludeExpense { get; set; }

        public string NoteKeyword { get; set; } = String.Empty;
    }
}
