using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSD.Services
{
    public class OrderDetails
    {
        public Int64 OrderId { get; set; }
        public Int64 OProductId { get; set; }
        public Int64 OUserId { get; set; }
        public string OrderTime { get; set; }
        public Int64 PProductId  { get; set; }
        public string PProductName { get; set; }
        public string PProductDescription { get; set; }
        public int PProductPrice { get; set; }
    }

}
