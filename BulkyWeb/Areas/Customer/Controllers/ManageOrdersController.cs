using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ManageOrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManageOrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<ManageOrders> manageOrdersList = _unitOfWork.ManageOrders.GetAll(includeProperties: "ApplicationUser").ToList();
            return View(manageOrdersList);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ManageOrders> manageOrderslist = _unitOfWork.ManageOrders.GetAll().ToList();
            return Json(new { data = manageOrderslist });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var manageOrdersToBeDeleted = _unitOfWork.ManageOrders.Get(id);
            if (manageOrdersToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.ManageOrders.Remove(manageOrdersToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion

    }
}




















//using BulkyBook.DataAccess.Repository.IRepository;
//using BulkyBook.Models.ViewModels;
//using BulkyBook.Models;
//using BulkyBook.Utility;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace BulkyBookWeb.Areas.Customer.Controllers
//{
//    [Area("Customer")]
//    public class ManageOrdersController : Controller
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public ManageOrdersController(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }
//        public IActionResult Index()
//        {
//            List<ManageOrders> manageOrdersList = _unitOfWork.ManageOrders.GetAll().ToList();
//            return View(manageOrdersList);
//        }

//        #region API CALLS
//        [HttpGet]
//        public IActionResult GetAll()
//        {
//            List<ManageOrders> manageOrderslist = _unitOfWork.ManageOrders.GetAll().ToList();
//            return Json(new { data = manageOrderslist });
//        }

//        [HttpDelete]
//        public IActionResult Delete(int id)
//        {
//            var manageOrdersToBeDeleted = _unitOfWork.ManageOrders.Get(id);
//            if (manageOrdersToBeDeleted == null)
//            {
//                return Json(new { success = false, message = "Error while deleting" });
//            }

//            _unitOfWork.ManageOrders.Remove(manageOrdersToBeDeleted);
//            _unitOfWork.Save();

//            return Json(new { success = true, message = "Delete Successful" });
//        }

//        #endregion


//        //[HttpGet]
//        //public IActionResult ManageOrders(int page = 1)
//        //{
//        //    // Lấy UserId của người dùng hiện tại
//        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//        //    // Số lượng đơn hàng trên mỗi trang
//        //    int pageSize = 10;

//        //    // Lấy danh sách các đơn hàng dựa trên UserId và phân trang
//        //    var orders = _unitOfWork.Order.GetAll(
//        //                    filter: o => o.ApplicationUserId == userId,
//        //                    includeProperties: "ApplicationUser")
//        //                 .OrderBy(o => o.Id) // Sắp xếp theo OrderId tăng dần
//        //                 .Skip((page - 1) * pageSize)
//        //                 .Take(pageSize)
//        //                 .Select(o => new ManageOrdersViewModel
//        //                 {
//        //                     OrderId = o.Id,
//        //                     UserName = o.ApplicationUser.UserName,
//        //                     PhoneNumber = o.ApplicationUser.PhoneNumber,
//        //                     Email = o.ApplicationUser.Email,
//        //                     OrderDate = o.OrderDate,
//        //                     OrderStatus = o.OrderStatus,
//        //                     OrderTotal = o.OrderTotal
//        //                 })
//        //                 .ToList();

//        //    // Tính toán số lượng đơn hàng
//        //    int totalCount = _unitOfWork.Order.GetAll(
//        //                        filter: o => o.ApplicationUserId == userId)
//        //                     .Count();

//        //    // Tạo đối tượng PaginatedList để truyền dữ liệu sang Razor view
//        //    var viewModel = new PaginatedList<ManageOrdersViewModel>(orders, totalCount, page, pageSize);

//        //    return View(viewModel);
//        //}
//    }
//}
