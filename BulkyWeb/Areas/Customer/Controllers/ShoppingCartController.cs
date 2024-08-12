using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace BulkyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Summary(string selectedIds)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            if (string.IsNullOrEmpty(selectedIds))
            {
                TempData["error"] = "You haven't selected any products to purchase.";
                return RedirectToAction(nameof(Cart));
            }

            var selectedIdList = selectedIds.Split(',').Select(int.Parse).ToList();
            var cartItems = _unitOfWork.CartItem.GetAll(
                u => u.ApplicationUserId == claim.Value && selectedIdList.Contains(u.Id),
                includeProperties: "Product"
            ).ToList();

            var order = new Order
            {
                ApplicationUserId = claim.Value,
                OrderDate = DateTime.Now,
                OrderTotal = cartItems.Sum(item => item.Quantity * item.Product.Price),
                OrderStatus = "Pending"
            };

            // Load ApplicationUser explicitly
            order.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == order.ApplicationUserId);

            var orderSummaryViewModel = new OrderSummaryViewModel
            {
                Order = order,
                OrderDetails = cartItems.Select(item => new OrderDetail
                {
                    ProductId = item.ProductId,
                    Product = item.Product,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                }).ToList()
            };

            return View(orderSummaryViewModel);
        }


        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var cartItem = _unitOfWork.CartItem.GetFirstOrDefault(
                u => u.ApplicationUserId == claim.Value && u.ProductId == productId,
                includeProperties: "Product"
            );

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ApplicationUserId = claim.Value,
                    ProductId = productId,
                    Quantity = quantity
                };
                _unitOfWork.CartItem.Add(cartItem);
                TempData["success"] = "Item added to cart successfully.";
            }
            else
            {
                cartItem.Quantity += quantity;
                TempData["success"] = "Quantity updated in cart successfully.";
            }

            _unitOfWork.Save();

            return RedirectToAction("Details", "Home", new { area = "Customer", productId = productId });
        }


        public IActionResult Cart()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var cartItems = _unitOfWork.CartItem.GetAll(
                u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product"
            ).ToList();

            foreach (var item in cartItems)
            {
                if (item.Product == null)
                {
                    item.Product = new Product { Title = "Unknown", Price = 0 };
                }
            }

            ShoppingCart shoppingCart = new ShoppingCart
            {
                Items = cartItems
            };

            return View(shoppingCart);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int cartItemId, int quantity)
        {
            var cartItem = _unitOfWork.CartItem.GetFirstOrDefault(u => u.Id == cartItemId, includeProperties: "Product");
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                _unitOfWork.CartItem.Update(cartItem);
                _unitOfWork.Save();

                var newItemTotal = cartItem.Quantity * cartItem.Product.Price;

                var newCartItems = _unitOfWork.CartItem.GetAll(u => u.ApplicationUserId == cartItem.ApplicationUserId, includeProperties: "Product");
                var newCartTotal = newCartItems.Sum(item => item.Quantity * item.Product.Price);

                var formattedNewItemTotal = string.Format("{0:C}", newItemTotal);
                var formattedNewCartTotal = string.Format("{0:C}", newCartTotal);

                return Json(new { success = true, newItemTotal = formattedNewItemTotal, newCartTotal = formattedNewCartTotal });
            }

            return Json(new { success = false });
        }



        [HttpPost]
        public IActionResult DecreaseQuantity(int cartItemId)
        {
            var cartItem = _unitOfWork.CartItem.GetFirstOrDefault(u => u.Id == cartItemId, includeProperties: "Product");
            if (cartItem != null && cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
                _unitOfWork.CartItem.Update(cartItem);
                _unitOfWork.Save();

                var newCartTotal = _unitOfWork.CartItem.GetAll(u => u.ApplicationUserId == cartItem.ApplicationUserId, includeProperties: "Product")
                                                       .Sum(item => item.Quantity * item.Product.Price);
                var newItemTotal = cartItem.Quantity * cartItem.Product.Price;

                return Json(new { success = true, newItemTotal, newCartTotal });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IncreaseQuantity(int cartItemId)
        {
            var cartItem = _unitOfWork.CartItem.GetFirstOrDefault(u => u.Id == cartItemId, includeProperties: "Product");
            if (cartItem != null)
            {
                cartItem.Quantity++;
                _unitOfWork.CartItem.Update(cartItem);
                _unitOfWork.Save();

                var newCartTotal = _unitOfWork.CartItem.GetAll(u => u.ApplicationUserId == cartItem.ApplicationUserId, includeProperties: "Product")
                                                       .Sum(item => item.Quantity * item.Product.Price);
                var newItemTotal = cartItem.Quantity * cartItem.Product.Price;

                return Json(new { success = true, newItemTotal, newCartTotal });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var cartItem = _unitOfWork.CartItem.GetFirstOrDefault(u => u.Id == cartItemId);
            if (cartItem != null)
            {
                _unitOfWork.CartItem.Remove(cartItem);
                _unitOfWork.Save();
                TempData["success"] = "Item removed from cart successfully.";
            }

            return RedirectToAction(nameof(Cart));
        }


        public IActionResult GetCartItemCount()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Json(new { count = 0 });
            }

            var cartItems = _unitOfWork.CartItem.GetAll(u => u.ApplicationUserId == claim.Value);
            var itemCount = cartItems.Count();

            return Json(new { count = itemCount });
        }

    }
}
