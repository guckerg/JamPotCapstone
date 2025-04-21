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
                    Quantity = item.Quantity
                }).ToList()
            };

            return View(viewModel);
        }

        public IActionResult AddToCart(int productId)
        {
            var itemToAdd = _context.Products.Find(productId);
            if (itemToAdd == null)
            {
                return NotFound();
            }

            var cartItems = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();

            var existingCartItem = cartItems.FirstOrDefault(item => item.ProductId == productId);
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

            return RedirectToAction("Index");
        }
    }
}
