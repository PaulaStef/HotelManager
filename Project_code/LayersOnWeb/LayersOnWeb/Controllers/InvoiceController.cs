using BusinessLayer.Contracts.Interfaces;
using BusinessLayer.Contracts.Models;
using LayersOnWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LayersOnWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        public readonly IInvoiceService InvoiceService; 
        public InvoiceController(IInvoiceService invoiceService)
        {
            InvoiceService = invoiceService;
        }

        [HttpPost("Send Invoice")]
        [Authorize(Roles = "Receptionist")]
        public string Post(Invoice invoice)
        {
            return InvoiceService.AddInvoice(new InvoiceModel
            {
                ReservationId = invoice.ReservationId,
                Message = invoice.Message,
                TotalPrice = 0
            }) ;
        }

        [HttpGet("Get this month invoices")]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetInvoices(string type)
        {
            return InvoiceService.getThisMonthInvoices(type);
        }
    }
}
