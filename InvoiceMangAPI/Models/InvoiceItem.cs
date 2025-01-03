using System;
using System.Collections.Generic;

namespace InvoiceMangAPI.Models
{
    public partial class InvoiceItem
    {
        public int ItemId { get; set; }
        public int? InvoiceId { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }

        public virtual Invoice Invoice { get; set; } = null!;
    }
}
