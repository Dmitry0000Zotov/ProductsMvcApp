using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsMvcApp.Domain.Entities
{
    public class EventLog
    {
        public Guid Id { get; set; }

        public DateTime EventDate { get; set; }

        public string? Description { get; set; }
    }
}
