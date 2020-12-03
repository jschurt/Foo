using Microsoft.Extensions.DependencyInjection;
using Solstice.Comms;

namespace Solstice
{
    public static class CommsServiceCollectionExtensions
    {
        public static void AddCommsBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IMessageBusinessLogic, MessageBusinessLogic>();
        }
    }
}
