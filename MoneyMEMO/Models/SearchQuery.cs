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

        public bool IsEmpty
        {
            get
            {
                bool hasDate = Date.HasValue || (StartDate.HasValue && EndDate.HasValue);
                bool hasAmount = Amount.HasValue || (MinAmount.HasValue && MaxAmount.HasValue);
                bool hasNote = !string.IsNullOrWhiteSpace(NoteKeyword);
                bool hasCategory = IncludeIncome != IncludeExpense; // 只选了收入或支出算非空

                return !(hasDate || hasAmount || hasNote || hasCategory);
            }
        }

        public bool Match(MoneyInfo m)
        {
            // 日期过滤
            if (UseDateRange)
            {
                if (StartDate.HasValue && m.Date < StartDate.Value) return false;
                if (EndDate.HasValue && m.Date > EndDate.Value) return false;
            }
            else if (Date.HasValue)
            {
                if (m.Date.Date != Date.Value.Date) return false;
            }

            // 金额过滤
            if (UseAmountRange)
            {
                if (MinAmount.HasValue && m.Amount < MinAmount.Value) return false;
                if (MaxAmount.HasValue && m.Amount > MaxAmount.Value) return false;
            }
            else if (Amount.HasValue)
            {
                if (m.Amount != Amount.Value) return false;
            }

            // 收入/支出过滤
            if ((!IncludeIncome && m.Category == MoneyCategory.Income) ||
                (!IncludeExpense && m.Category == MoneyCategory.Expense))
            {
                return false;
            }

            // 备注关键字
            if (!string.IsNullOrWhiteSpace(NoteKeyword) &&
                !m.Note.Contains(NoteKeyword, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
