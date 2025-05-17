using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;
using JampotCapstone.Data;
using JampotCapstone.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IEmailSender _emailSender;
        private ITextElementRepository _repo;

        public ContactUsController(IEmailSender emailSender, ITextElementRepository r)
        {
            _emailSender = emailSender;
            _repo = r;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Contact = await _repo.GetTextElementsByPageAsync("contact");
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
