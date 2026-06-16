using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tekus.Application.Common.Behaviors;
using Tekus.Application.Features.Services.Events;
using Tekus.Domain.Events;
using Tekus.Domain.Interfaces;

namespace Tekus.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            services.AddScoped<IDomainEventHandler<ServiceCreatedEvent>, ServiceCreatedEmailNotificationHandler>();

            return services;
        }
    }
}
