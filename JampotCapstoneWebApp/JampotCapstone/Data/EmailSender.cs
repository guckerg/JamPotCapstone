using JampotCapstone.Models;
using System.Net;
using System.Net.Mail;

namespace JampotCapstone.Data
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(Message model)
        {
            //define sender and receiver
            var recieveEmail = "jampottesting@gmail.com";
            var email = "jampottesting@gmail.com";
            var pw = "Secret123!";

            //set up smtp with credentials
            var client = new SmtpClient("smtp.gamil.com", 587) //finish setting up smtp
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(email, pw)
            };

            //send the eamil
            return client.SendMailAsync(
                new MailMessage(
                    from: email,
                    to: recieveEmail,
                    model.Subject,
                    "Name: " + model.Name + ". Phone Number: " +
                    model.PhoneNumber + ". Email: " + model.Email + ". Message: " + model.MessageText));
        }
    }
}
