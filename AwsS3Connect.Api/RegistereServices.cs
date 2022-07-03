using AwsS3Connect.Core.ServiceReferences;
using AwsS3Connect.Domain.ServiceReferences;
using MediatR;

namespace AwsS3Connect.Api
{
    public static class RegistereServices
    {
        public static IServiceCollection AddOtherServices(this IServiceCollection services)
        {
            services.AddCore();
            services.AddDomain();

            return services;
        }
    }
}