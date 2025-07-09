using System.Threading.Tasks;

namespace BLL.ServiceAbstraction
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
} 