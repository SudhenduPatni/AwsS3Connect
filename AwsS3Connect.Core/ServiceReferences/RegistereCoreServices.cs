using AwsS3Connect.Core.AwsHelper;
using Microsoft.Extensions.DependencyInjection;

namespace AwsS3Connect.Core.ServiceReferences
{
    public static class RegistereCoreServices
    {
        public static void AddCore(this IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("AwsS3Connect.Core");
            services.AddSingleton(assembly);
            services.AddTransient<IAwsS3Config, AwsS3Config>();
        }
    }
}
