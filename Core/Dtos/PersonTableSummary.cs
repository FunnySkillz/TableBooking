using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class PersonTableSummary
    {
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;

        public string QRCode { get; set; } = String.Empty;
        public string TableName { get; set; } = String.Empty;
    }
}
