using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;

namespace JampotCapstone.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public void SendMessage()
        {
            //CRUD for contact message
        }
    }
}
