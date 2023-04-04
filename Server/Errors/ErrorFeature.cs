namespace ErrorDetailsRFC7807.Server.Errors;

public record ErrorFeature(ErrorType ErrorType, string? Title = null, string? Detail = null);

public enum ErrorType
{
    ArgumentException,
    InvalidOperationException,
    NotFoundException
}
