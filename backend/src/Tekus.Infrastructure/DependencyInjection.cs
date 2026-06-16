using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tekus.Application.Common.Auth;
using Tekus.Application.Common.Options;
using Tekus.Domain.Interfaces;
using Tekus.Infrastructure.Data;
using Tekus.Infrastructure.Events;
using Tekus.Infrastructure.Security;

namespace Tekus.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IEventDispatcher, InMemoryEventDispatcher>();

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.Configure<DefaultUserSettings>(configuration.GetSection(DefaultUserSettings.SectionName));

            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
