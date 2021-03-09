using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrsServer.Models {
    public class RequestLine {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public uint Quantity { get; set; } = 1;




        public RequestLine() { }
    }
}
