using JampotCapstone.Data;
using JampotCapstone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly List<OrderItem> _cartItems;

        public CartController(ApplicationDbContext context, List<OrderItem> cartItems) {
            _context = context;
            _cartItems = cartItems;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddToCart(int id)
        {
            var itemToAdd = _context.OrderItems.Find(id);

            var existingCartItem = _cartItems.FirstOrDefault(item => item.Product.ProductId == id);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity++;
            }
            else
            {
                _cartItems.Add(new OrderItem
                {
                    Product = itemToAdd,
                    Quantity = 1
                });
            }
            return View();
        }
    }
}
