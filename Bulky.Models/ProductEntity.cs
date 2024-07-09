using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public string Product { get ; set; }
        public long Rate { get; set; }
        public long Quanity { get; set; }
        public string ImagePath { get; set; }

    }
}
