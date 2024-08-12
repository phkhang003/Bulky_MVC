using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BulkyBookWeb.Controllers
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
        public IActionResult PlaceOrder(OrderSummaryViewModel orderSummaryViewModel)
        {
            // Kiểm tra kiểu thanh toán
            if (orderSummaryViewModel.PaymentMethod == "PaymentOnDelivery")
            {
                // Thực hiện lưu đơn hàng vào cơ sở dữ liệu
                var order = new Order
                {
                    ApplicationUserId = orderSummaryViewModel.Order.ApplicationUserId,
                    FullName = orderSummaryViewModel.Order.ApplicationUser.FullName,
                    PhoneNumber = orderSummaryViewModel.Order.ApplicationUser.PhoneNumber,
                    Email = orderSummaryViewModel.Order.ApplicationUser.Email,
                    OrderTotal = orderSummaryViewModel.Order.OrderTotal,
                    OrderStatus = "Pending",
                    PaymentStatus = "PaymentOnDelivery",
                    OrderDate = DateTime.Now
                };

                _unitOfWork.Order.Add(order);
                _unitOfWork.Save();

                foreach (var item in orderSummaryViewModel.OrderDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Price = item.Price,
                        Quantity = item.Quantity
                    };

                    _unitOfWork.OrderDetail.Add(orderDetail);
                }
                _unitOfWork.Save();

                // Chuyển hướng sang trang xác nhận đơn hàng
                return RedirectToAction("PaymentOnDelivery", "Order", new { id = order.Id });
            }
            else if (orderSummaryViewModel.PaymentMethod == "OnlinePayment")
            {
                // Chuyển hướng sang trang thanh toán trực tuyến
                return RedirectToAction("OnlinePayment", "Payment");
            }

            // Mặc định chuyển hướng sang trang giỏ hàng nếu không có kiểu thanh toán phù hợp
            return RedirectToAction("Cart", "ShoppingCart");
        }

        public IActionResult PaymentOnDelivery(int id)
        {
            var order = _unitOfWork.Order.GetFirstOrDefault(u => u.Id == id, includeProperties: "OrderDetails.Product");
            return View(order);
        }

        public IActionResult OnlinePayment()
        {
            // Trả về view cho thanh toán trực tuyến
            return View();
        }
    }
}
















//[HttpPost]
//public IActionResult CreateOrder(OrderSummaryViewModel model)
//{
//    if (model.Order == null)
//    {
//        // Handle null Order case, return an error message or redirect back to the summary page
//        return RedirectToAction("Summary");
//    }

//    var claimsIdentity = (ClaimsIdentity)User.Identity;
//    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

//    if (claim == null)
//    {
//        return RedirectToAction("Login", "Account", new { area = "Identity" });
//    }

//    var order = new Order
//    {
//        ApplicationUserId = claim.Value,
//        OrderDate = DateTime.Now,
//        OrderTotal = model.Order.OrderTotal,
//        OrderStatus = "Pending",
//        PaymentStatus = "Pending"
//    };

//    _unitOfWork.Order.Add(order);
//    _unitOfWork.Save();

//    foreach (var item in model.OrderDetails)
//    {
//        var orderDetail = new OrderDetail
//        {
//            OrderId = order.Id,
//            ProductId = item.Product.Id,
//            Quantity = item.Quantity,
//            Price = item.Price
//        };

//        _unitOfWork.OrderDetail.Add(orderDetail);
//    }

//    _unitOfWork.Save();

//    var cartItems = _unitOfWork.CartItem.GetAll(u => u.ApplicationUserId == claim.Value).ToList();
//    _unitOfWork.CartItem.RemoveRange(cartItems);
//    _unitOfWork.Save();

//    return RedirectToAction("OrderConfirmation", "Order", new { orderId = order.Id });
//}


//public IActionResult OrderConfirmation(int orderId)
//{
//    var order = _unitOfWork.Order.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser");

//    if (order == null)
//    {
//        return NotFound();
//    }

//    return View(order);
//}