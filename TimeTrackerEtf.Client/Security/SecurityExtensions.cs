using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace TimeTrackerEtf.Client.Security
{
    public static class SecurityExtensions
    {
        public static void AddTokenAuthenticationStateProvider(
            this IServiceCollection services)
        {
            services.AddScoped<TokenAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(
                provider => provider.GetRequiredService<TokenAuthenticationStateProvider>());
        }
    }
}
