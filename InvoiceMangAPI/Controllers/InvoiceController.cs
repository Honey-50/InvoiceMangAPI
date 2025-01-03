//using InvoiceMangAPI.Data;
//using InvoiceMangAPI.Models;
//using InvoiceMangAPI.Repositories;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace InvoiceMangAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class InvoiceController : ControllerBase
//    {
//        private readonly IRepository<Invoice> _invoiceRepository;

//        public InvoiceController(IRepository<Invoice> invoiceRepository)
//        {
//            _invoiceRepository = invoiceRepository;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetInvoices()
//        {
//            var invoices = await _invoiceRepository.GetAllAsync();
//            return Ok(invoices);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetInvoice(int id)
//        {
//            var invoice = await _invoiceRepository.GetByIdAsync(id);
//            if (invoice == null) return NotFound();
//            return Ok(invoice);
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateInvoice([FromBody] Invoice invoice)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);
//            await _invoiceRepository.AddAsync(invoice);
//            await _invoiceRepository.SaveChangesAsync();
//            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.InvoiceId }, invoice);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] Invoice invoice)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);
//            var existingInvoice = await _invoiceRepository.GetByIdAsync(id);
//            if (existingInvoice == null) return NotFound();

//            existingInvoice.CustomerName = invoice.CustomerName;
//            existingInvoice.TotalAmount = invoice.TotalAmount;
//            existingInvoice.Status = invoice.Status;

//            _invoiceRepository.Update(existingInvoice);
//            await _invoiceRepository.SaveChangesAsync();

//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteInvoice(int id)
//        {
//            var invoice = await _invoiceRepository.GetByIdAsync(id);
//            if (invoice == null) return NotFound();

//            _invoiceRepository.Remove(invoice);
//            await _invoiceRepository.SaveChangesAsync();

//            return NoContent();
//        }
//    }
//}


using InvoiceMangAPI.Models;
//using InvoiceMangAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvoiceMangAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceDBContext _context;

        public InvoiceController(InvoiceDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return await _context.Invoices.Include(i => i.InvoiceItems).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            var invoice = await _context.Invoices.Include(i => i.InvoiceItems).FirstOrDefaultAsync(i => i.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }
            return invoice;
        }

        //[HttpPost]
        //[Authorize]
        //public async Task<ActionResult<Invoice>> CreateInvoice(Invoice invoice)
        //{
        //    _context.Invoices.Add(invoice);
        //    await _context.SaveChangesAsync();
        //    return CreatedAtAction(nameof(GetInvoice), new { id = invoice.InvoiceId }, invoice);
        //}

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Invoice>> CreateInvoice(Invoice invoice)
        {
            if (invoice == null)
            { return BadRequest("Invoice is required."); }
            _context.Invoices.Add(invoice); await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.InvoiceId }, invoice);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateInvoice(int id, Invoice invoice)
        {
            if (id != invoice.InvoiceId)
            {
                return BadRequest();
            }
            _context.Entry(invoice).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

