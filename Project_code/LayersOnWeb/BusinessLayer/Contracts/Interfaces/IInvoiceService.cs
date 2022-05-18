using BusinessLayer.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts.Interfaces
{
    public interface IInvoiceService
    {
        string AddInvoice(InvoiceModel room);
        IActionResult getThisMonthInvoices(string type);
    }
}
