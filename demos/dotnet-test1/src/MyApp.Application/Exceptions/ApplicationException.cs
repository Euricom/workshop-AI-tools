namespace MyApp.Application.Exceptions;

public abstract class ApplicationException : Exception
{
    protected ApplicationException(string message) : base(message) { }
}

public class NotFoundException : ApplicationException
{
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.") { }
}

public class ValidationException : ApplicationException
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation failures have occurred.")
    {
        Errors = errors;
    }
}

public class AuthenticationException : ApplicationException
{
    public AuthenticationException() : base("Authentication failed.") { }
}

public class AuthorizationException : ApplicationException
{
    public AuthorizationException() : base("User is not authorized to perform this action.") { }
}

public class BusinessRuleException : ApplicationException
{
    public BusinessRuleException(string message) : base(message) { }
}

public class DuplicateEntityException : ApplicationException
{
    public DuplicateEntityException(string name, object key)
        : base($"Entity \"{name}\" ({key}) already exists.") { }
}