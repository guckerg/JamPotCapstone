using JampotCapstone.Models;

namespace JampotCapstone.Data.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message model);
    }
}
