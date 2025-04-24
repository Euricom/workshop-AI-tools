using MediatR;
using Microsoft.Extensions.Logging;
using MyApp.Application.Interfaces;

namespace MyApp.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly ICurrentUserService _currentUserService;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId ?? "anonymous";

        _logger.LogInformation(
            "Handling {RequestName} for user {UserId}",
            requestName,
            userId);

        try
        {
            var response = await next();

            _logger.LogInformation(
                "Handled {RequestName} for user {UserId}",
                requestName,
                userId);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error handling {RequestName} for user {UserId}",
                requestName,
                userId);

            throw;
        }
    }
}