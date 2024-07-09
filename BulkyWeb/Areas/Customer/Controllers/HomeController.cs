using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string searchString)
        {
            IEnumerable<Product> productList;

            if (!string.IsNullOrEmpty(searchString))
            {
                productList = _unitOfWork.Product.GetAll(includeProperties: "Category")
                                .Where(p => p.Title.StartsWith(searchString, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            }

            return View(productList);
        }

        public IActionResult SearchProducts(string searchString)
        {
            IEnumerable<Product> productList;

            if (!string.IsNullOrEmpty(searchString))
            {
                productList = _unitOfWork.Product.GetAll(includeProperties: "Category")
                                .Where(p => p.Title.StartsWith(searchString, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                productList = Enumerable.Empty<Product>();
            }

            var titles = productList.Select(p => p.Title).ToList();
            return Json(titles);
        }



        public IActionResult Details(int productId)
        {
            Product product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productId, includeProperties: "Category");
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
