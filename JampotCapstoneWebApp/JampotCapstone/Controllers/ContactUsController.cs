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
        private ITextElementRepository _repo;

        public ContactUsController(IEmailSender emailSender, ApplicationDbContext ctx, ITextElementRepository r)
        {
            _emailSender = emailSender;
            _context = ctx;
            _repo = r;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Contact = await _repo.GetTextElementsByPage("contact");
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
