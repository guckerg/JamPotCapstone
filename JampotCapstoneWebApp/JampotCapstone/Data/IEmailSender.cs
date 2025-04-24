using JampotCapstone.Models;

namespace JampotCapstone.Data
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message model);
    }
}
