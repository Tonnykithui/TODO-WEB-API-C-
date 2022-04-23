using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTODO.Models
{
    public class ValidationMessage
    {
        public bool IsError { get; set; }

        public string Message { get; set; }

        public IEnumerable<string> Error { get; set; }

        public DateTime? Expiry { get; set; }
    }
}
