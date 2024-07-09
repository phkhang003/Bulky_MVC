using System;
using System.Collections.Generic;

namespace BulkyBook.Models.ViewModels
{
    public class OrderHistoryViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public double OrderTotal { get; set; }
        public string OrderStatus { get; set; }
    }
}
