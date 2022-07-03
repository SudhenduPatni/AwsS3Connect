using AwsS3Connect.Infra.Interface;
using AwsS3Connect.Infra.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace AwsS3Connect.Domain.ServiceReferences
{
    public static class RegistereDomainServices
    {
        public static void AddDomain(this IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("AwsS3Connect.Domain");
            services.AddAutoMapper(assembly);
            services.AddSingleton(assembly);
            services.AddTransient<IAwsS3Repository, AwsS3Repository>();
        }
    }
}
