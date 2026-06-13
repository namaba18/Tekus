using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Tekus.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[Start] Handling {RequestName} - Response={Response}", typeof(TRequest).Name, typeof(TResponse).Name);

            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            timer.Stop();
            var timeTaken = timer.Elapsed;
            if (timeTaken.Seconds > 3)
            {
                logger.LogWarning("[Performance] Handling {RequestName} took {TimeTaken} seconds - Response={Response}", typeof(TRequest).Name, timeTaken.TotalSeconds, typeof(TResponse).Name);
            }

            logger.LogInformation("[End] Handling {RequestName} - Response={Response} - TimeTaken={TimeTaken} seconds", typeof(TRequest).Name, typeof(TResponse).Name, timeTaken.TotalSeconds);

            return response;
        }
    }
}
