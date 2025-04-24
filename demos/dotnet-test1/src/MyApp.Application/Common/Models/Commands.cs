using MediatR;
using MyApp.Application.Common.Models;

namespace MyApp.Application.Common.Models;

public interface ICommand : IRequest<Result<Unit>> { }

public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }

public abstract class CommandBase : ICommand
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}

public abstract class CommandBase<TResponse> : ICommand<TResponse>
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result<Unit>>
    where TCommand : ICommand
{ }

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{ }

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }

public abstract class QueryBase<TResponse> : IQuery<TResponse>
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{ }

public abstract class PaginatedQueryBase : QueryBase<PaginatedResult<object>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public abstract class PaginatedQueryBase<TResponse> : QueryBase<PaginatedResult<TResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}