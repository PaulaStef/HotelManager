using BusinessLayer.Contracts.Factory.Docs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts.Factory
{
    public class DocumentFactory : IDocumentFactory
    {
        public IDocument CreateExport(string type)
        {
            switch (type)
            {
                case "XML":
                    return new XmlDocument();
                case "CSV":
                    return new CSVDocument();
                case "JSON":
                    return new JsonDocument();

                default:
                    return null;
            }
        }
    }
}
