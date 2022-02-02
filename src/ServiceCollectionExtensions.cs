using System;
using System.Linq;
using EPiCode.InspectInIndex;
using EPiServer.Shell.Modules;
using Microsoft.AspNetCore.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private static readonly Action<AuthorizationPolicyBuilder> DefaultPolicy = p => p.RequireRole("Administrators","CmsAdmins", "WebAdmins");

        public static IServiceCollection AddInspectInIndex(
            this IServiceCollection services)
        {
            return services.AddInspectInIndex(DefaultPolicy);
        }

        public static IServiceCollection AddInspectInIndex(
            this IServiceCollection services, Action<AuthorizationPolicyBuilder> configurePolicy)
        {
            
            services.Configure<ProtectedModuleOptions>(
                pm =>
                {
                    if (!pm.Items.Any(i => i.Name.Equals("EPiCode.InspectInIndex", StringComparison.OrdinalIgnoreCase)))
                    {
                        pm.Items.Add(new ModuleDetails { Name = "EPiCode.InspectInIndex" });
                    }
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.PolicyName, configurePolicy);
            });

            return services;
        }
    }
}