using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
using JampotCapstone.Data;

namespace JampotCapstone.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IEmailSender _emailSender;

        public ContactUsController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SendMessage(Message model)
        {
            await _emailSender.SendEmailAsync(model);

            return View("Index");
        }
    }
}
