using BusinessLayer.Contracts.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLayer.Contracts.Factory
{
    public interface IDocument
    {
        IActionResult Export<T>(List<T> entities) where T : class;
        IActionResult Export(List<string> clients);
    }
}
