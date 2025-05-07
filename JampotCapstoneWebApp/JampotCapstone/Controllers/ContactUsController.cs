using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
using JampotCapstone.Data;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IEmailSender _emailSender;
        private ApplicationDbContext _context;

        public ContactUsController(IEmailSender emailSender, ApplicationDbContext ctx)
        {
            _emailSender = emailSender;
            _context = ctx;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Contact = await _context.TextElements.Where(t => t.Location.ToLower().Contains("contact")).ToListAsync();
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
