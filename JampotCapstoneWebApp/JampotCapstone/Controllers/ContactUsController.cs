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
            if(ModelState.IsValid)
            {
                await _emailSender.SendEmailAsync(model);

                TempData["SuccessMessage"] = "Your message was successfully sent! Thank you!";
            }

            return View("Index");
        }
    }
}
