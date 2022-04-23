using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTODO.Models
{
    public class Todo
    {
        public int TodoId { get; set; }

        public string Completed { get; set; } 

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public DateTime EntryDate { get; set; }
    }
}
