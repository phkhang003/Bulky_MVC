using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;

namespace BulkyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IActionResult CreateOrder(OrderSummaryViewModel model)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var order = new Order
            {
                ApplicationUserId = claim.Value,
                OrderDate = DateTime.Now,
                OrderTotal = model.Order.OrderTotal,
                OrderStatus = "Pending",
                PaymentStatus = "Pending"
            };

            _unitOfWork.Order.Add(order);
            _unitOfWork.Save();

            foreach (var item in model.OrderDetails)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    Price = item.Price
                };

                _unitOfWork.OrderDetail.Add(orderDetail);
            }

            _unitOfWork.Save();

            // Clear the cart after creating the order
            var cartItems = _unitOfWork.CartItem.GetAll(u => u.ApplicationUserId == claim.Value).ToList();
            _unitOfWork.CartItem.RemoveRange(cartItems);
            _unitOfWork.Save();

            // Redirect to the Order Confirmation view
            return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }

        public IActionResult OrderConfirmation(int orderId)
        {
            var order = _unitOfWork.Order.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser");

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        //[HttpGet]
        //public IActionResult ManageOrders(int page = 1)
        //{
        //    // Lấy UserId của người dùng hiện tại
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    // Số lượng đơn hàng trên mỗi trang
        //    int pageSize = 10;

        //    // Lấy danh sách các đơn hàng dựa trên UserId và phân trang
        //    var orders = _unitOfWork.Order.GetAll(
        //                    filter: o => o.ApplicationUserId == userId,
        //                    includeProperties: "ApplicationUser")
        //                 .OrderBy(o => o.Id) // Sắp xếp theo OrderId tăng dần
        //                 .Skip((page - 1) * pageSize)
        //                 .Take(pageSize)
        //                 .Select(o => new ManageOrdersViewModel
        //                 {
        //                     OrderId = o.Id,
        //                     UserName = o.ApplicationUser.UserName,
        //                     PhoneNumber = o.ApplicationUser.PhoneNumber,
        //                     Email = o.ApplicationUser.Email,
        //                     OrderDate = o.OrderDate,
        //                     OrderStatus = o.OrderStatus,
        //                     OrderTotal = o.OrderTotal
        //                 })
        //                 .ToList();

        //    // Tính toán số lượng đơn hàng
        //    int totalCount = _unitOfWork.Order.GetAll(
        //                        filter: o => o.ApplicationUserId == userId)
        //                     .Count();

        //    // Tạo đối tượng PaginatedList để truyền dữ liệu sang Razor view
        //    var viewModel = new PaginatedList<ManageOrdersViewModel>(orders, totalCount, page, pageSize);

        //    return View(viewModel);
        //}


    }
}
