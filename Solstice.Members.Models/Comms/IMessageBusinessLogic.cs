using Solstice.Comms.MessageModels;
using System.Threading.Tasks;

namespace Solstice.Comms
{
    public interface IMessageBusinessLogic
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
