using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSD.Services
{
    public class OrderProperties
    {
            public Int64 OrderId { get; set; }
            public Int64 ProductId { get; set; }
            public Int64 UserId { get; set; }
            public DateTime Time { get; set; }
            public string status { get; set; }
    }
}
