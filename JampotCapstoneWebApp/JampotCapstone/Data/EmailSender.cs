using JampotCapstone.Models;
using System.Net;
using System.Net.Mail;
using JampotCapstone.Data.Interfaces;

namespace JampotCapstone.Data
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(Message model)
        {
            //define sender and receiver
            var recieveEmail = "jampotTesting@gmail.com";
            var sendEmail = "jampotTesting@gmail.com";
            var pw = "snhu jfca qkva mnhr"; //smtp substitute password

            //set up smtp with credentials
            var client = new SmtpClient("smtp.gmail.com", 587) //finish setting up smtp
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(sendEmail, pw)
            };

            //send the eamil
            return client.SendMailAsync(
                new MailMessage(
                    from: sendEmail,
                    to: recieveEmail,
                    model.Subject,
                    "Name: " + model.Name + ". Phone Number: " +
                    model.PhoneNumber + ". Email: " + model.Email + ". Message: " + model.MessageText));
        }
    }
}
