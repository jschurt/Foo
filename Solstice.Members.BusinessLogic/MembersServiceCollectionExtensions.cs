using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solstice.Members;
using System.Net.Http;

namespace Solstice
{
    public static class MembersServiceCollectionExtensions
    {
        public static void AddMembersBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMemberBusinessLogic, MemberBusinessLogic>();
            services.AddScoped<IMemberContactBusinessLogic, MemberContactBusinessLogic>();
            services.AddScoped<IMemberAddressBusinessLogic, MemberAddressBusinessLogic>();
            services.AddScoped<IMemberIdCardBusinessLogic, MemberIdCardBusinessLogic>();
            services.AddScoped<IMemberDateOfLastCleaningBusinessLogic, MemberDateOflastCleaningBusinessLogic>();
            services.AddScoped<IBenefitBusinessLogic, BenefitBusinessLogic>();
            services.AddScoped<IEnrollmentBusinessLogic, EnrollmentBusinessLogic>();
            services.AddScoped<IPassiveEnrollmentBusinessLogic, PassiveEnrollmentBusinessLogic>();
            services.AddScoped<IWelcomePacketBusinessLogic, WelcomePacketBusinessLogic>();
            services.AddScoped<IMemberVoeBusinessLogic, MemberVoeBusinessLogic>();

            services.Configure<BenefitBusinessLogicSettings>(configuration.GetSection("Benefit"));
            services.Configure<MemberVoeBusinessLogicSettings>(configuration.GetSection("VOE"));

            services.AddHttpClient(MemberVoeBusinessLogic.HttpClientName_SsrsVoeRequest)
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new HttpClientHandler()
                    {
                        UseDefaultCredentials = true
                    };
                });
        }

    }
}
