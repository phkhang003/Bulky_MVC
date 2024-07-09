using System;
using System.Collections.Generic;

namespace BulkyBook.Models.ViewModels
{
    public class ManageOrdersViewModel
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime OrderDate { get; set; }
        public double OrderTotal { get; set; }
        public string OrderStatus { get; set; }
    }
}
