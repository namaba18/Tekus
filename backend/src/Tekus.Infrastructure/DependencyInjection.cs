using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tekus.Application.Common.Auth;
using Tekus.Application.Common.Email;
using Tekus.Application.Common.Interfaces;
using Tekus.Application.Common.Options;
using Tekus.Domain.Interfaces;
using Tekus.Domain.Interfaces.Repositories;
using Tekus.Infrastructure.Data;
using Tekus.Infrastructure.Email;
using Tekus.Infrastructure.Events;
using Tekus.Infrastructure.Repositories;
using Tekus.Infrastructure.Security;

namespace Tekus.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

            services.AddScoped<IEventDispatcher, InMemoryEventDispatcher>();

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.Configure<DefaultUserSettings>(configuration.GetSection(DefaultUserSettings.SectionName));
            services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));
            services.Configure<NotificationSettings>(configuration.GetSection(NotificationSettings.SectionName));

            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IEmailSender, SmtpEmailSender>();

            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();

            return services;
        }
    }
}
