using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace BulkyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CheckOutController : Controller
    {
        public IActionResult Index()
        {
            List<ProductEntity> productList = new List<ProductEntity>();
            productList = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Product = "Tommy Hilfiger",
                    Rate = 1500,
                    Quanity = 2,
                    ImagePath = "images/Image1.jpg"
                },
                new ProductEntity
                {
                    Product = "TimeWear",
                    Rate = 1000,
                    Quanity = 1,
                    ImagePath = "images/Image2.jpg"
                },
            };
            return View(productList);
        }
      
    }
}
