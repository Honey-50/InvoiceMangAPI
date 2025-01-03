using System;
using System.Collections.Generic;

namespace InvoiceMangAPI.Models
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceItems = new HashSet<InvoiceItem>();
        }
        public int InvoiceId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
