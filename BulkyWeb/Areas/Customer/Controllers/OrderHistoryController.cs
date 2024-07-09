using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BulkyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderHistoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderHistoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var orders = _unitOfWork.Order.GetAll(o => o.ApplicationUserId == claim.Value)
                            .Select(o => new OrderHistoryViewModel
                            {
                                OrderId = o.Id,
                                OrderDate = o.OrderDate,
                                OrderTotal = o.OrderTotal,
                                OrderStatus = o.OrderStatus
                            }).ToList();

            return View(orders);
        }
    }
}
