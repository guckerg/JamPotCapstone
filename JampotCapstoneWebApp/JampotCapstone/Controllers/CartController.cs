using JampotCapstone.Data;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "CartItems";

        public CartController(ApplicationDbContext context) {
            _context = context;
        }
        public IActionResult Index()
        {
            var cartItems = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();

            var viewModel = new CartViewModel
            {
                Items = cartItems.Select(item => new CartItemViewModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.ProductName ?? "",
                    ProductPrice = item.Product.ProductPrice,
                    ProductPhoto = item.Product.ProductPhoto,
                    Quantity = item.Quantity
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddToCart([FromBody] AddToCartRequest request)
        {

            var itemToAdd = _context.Products
                .Include(p => p.ProductPhoto)
                .SingleOrDefault(p => p.ProductId == request.ProductId);

            if (itemToAdd == null)
            {
                return NotFound();
            }

            var cartItems = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();

            var existingCartItem = cartItems.FirstOrDefault(item => item.ProductId == request.ProductId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity++;
            }
            else
            {
                cartItems.Add(new OrderItem
                {
                    ProductId = itemToAdd.ProductId,
                    Product = itemToAdd,
                    Quantity = 1
                });
            }

            HttpContext.Session.SetObjectAsJson(CartSessionKey, cartItems);
            var totalCartQuantity = cartItems.Sum(i => i.Quantity);

            // Store a message in TempData for the next request
            TempData["AddToCartMessage"] = "Item added to your cart!";
            TempData["NotificationType"] = "success";

            return Json(new { success = true, totalCartQuantity });
        }

        //class to let the method pass a Json object as an id
        public class AddToCartRequest
        {
            public int ProductId { get; set; }
        }

        [HttpGet]
        // Used to display the correct amount of items on the cart badge
        public IActionResult GetCartQuantity()
        {
            var cartItems = HttpContext.Session.GetObjectFromJson<List<OrderItem>>("CartItems") ?? new List<OrderItem>();
            var totalCartQuantity = cartItems.Sum(i => i.Quantity);

            return Json(new { totalCartQuantity });
        }

        [HttpPost]
        public IActionResult RemoveFromCart([FromBody] AddToCartRequest request)
        {
            var cartItems = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();

            var item = cartItems.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                {
                    cartItems.Remove(item);
                }

                HttpContext.Session.SetObjectAsJson(CartSessionKey, cartItems);
            }

            var totalCartQuantity = cartItems.Sum(i => i.Quantity);

            return Json(new { success = true, totalCartQuantity });
        }

        public class RemoveFromCartRequest
        {
            public int ProductId { get; set; }
        }
    }
}
