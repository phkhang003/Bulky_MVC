//using BulkyBook.DataAccess.Repository.IRepository;
//using BulkyBook.Models;
//using BulkyBook.Models.ViewModels;
//using Microsoft.AspNetCore.Mvc;

//namespace BulkyBookWeb.Areas.Customer.Controllers
//{
//    [Area("Customer")]
//    public class PaymentController : Controller
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public PaymentController(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        [HttpPost]
//        public IActionResult PlaceOrder(OrderSummaryViewModel orderSummaryViewModel)
//        {
//            // Kiểm tra kiểu thanh toán
//            if (orderSummaryViewModel.PaymentMethod == "PaymentOnDelivery")
//            {
//                // Thực hiện lưu đơn hàng vào cơ sở dữ liệu
//                var order = new Order
//                {
//                    // Khởi tạo các thuộc tính cần thiết
//                    ApplicationUserId = orderSummaryViewModel.Order.ApplicationUserId,
//                    FullName = orderSummaryViewModel.Order.ApplicationUser.FullName,
//                    PhoneNumber = orderSummaryViewModel.Order.ApplicationUser.PhoneNumber,
//                    Email = orderSummaryViewModel.Order.ApplicationUser.Email,
//                    OrderTotal = orderSummaryViewModel.Order.OrderTotal,
//                    OrderStatus = "Pending",
//                    PaymentStatus = "PaymentOnDelivery",
//                    OrderDate = DateTime.Now
//                };

//                _unitOfWork.Order.Add(order);
//                _unitOfWork.Save();

//                foreach (var item in orderSummaryViewModel.OrderDetails)
//                {
//                    var orderDetail = new OrderDetail
//                    {
//                        OrderId = order.Id,
//                        ProductId = item.ProductId,
//                        Price = item.Price,
//                        Quantity = item.Quantity
//                    };

//                    _unitOfWork.OrderDetail.Add(orderDetail);
//                }
//                _unitOfWork.Save();

//                // Chuyển hướng sang trang xác nhận đơn hàng
//                return RedirectToAction("OrderConfirmation", "Order", new { id = order.Id });
//            }
//            else if (orderSummaryViewModel.PaymentMethod == "OnlinePayment")
//            {
//                // Chuyển hướng sang trang thanh toán trực tuyến
//                return RedirectToAction("OnlinePayment", "Payment");
//            }

//            // Mặc định chuyển hướng sang trang giỏ hàng nếu không có kiểu thanh toán phù hợp
//            return RedirectToAction("Cart", "ShoppingCart");
//        }

//        public IActionResult OnlinePayment()
//        {
//            // Hiển thị trang thanh toán trực tuyến
//            return View();
//        }
//    }
//}
