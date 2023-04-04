# ErrorDetailsRFC7807

Launch and test several modes of invoking errors in Server\HttpTools\WeatherForecastController.http like:

![image](https://user-images.githubusercontent.com/12967802/229808994-72fbf08c-d9f4-4e09-a34b-76c9814a08e3.png)

See "Settings" chapter at the end of this file.

## Context

I have use 4 ways of generating errors based on this document https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-7.0

- Exception (/WeatherForecast/SimpleException)
Result response is compliant to standard definition of AddProblemDetails extensions based on RFC7807

- Error using this feature on IActionResult return type (/WeatherForecast/ErrorFeatureOnIActionResult)
Result Response is compliant to customized defintion of AddProblemDetails extensions.

- Error using this feature on Results return type (/WeatherForecast/ErrorFeatureOnResults)
No body in result Response, it is not compliant to customized defintion of AddProblemDetails extensions.

- Error not using feature or ErrorType based on Results return type but using TypedResult.Problem (WeatherForecast/ErrorFeatureOnResultsUsingTypedResultsProblem)
Result Response is compliant to standard definition of AddProblemDetails extensions

## Conclusion

My conclusion is that you can use this settings to customize errors by 3 ways:
- Using Exception and AddProblemDetails
- Using IActionResult and AddProblemDetails + ErrorFeature customization
- Using Results, TypedResult.Problem and AddProblemDetails

When possible, I think that the 3rd way of writting API is the preferred one due to contract compilation on possible return types and the easy way of creating its own details.

## Settings

In Program.cs

```
builder.Services.AddProblemDetails(options =>
    options.CustomizeProblemDetails = ctx => // Add custom problem details
    {
      var errorFeature = ctx.HttpContext.Features.Get<ErrorFeature>();
      if (errorFeature is not null)
      {
        ctx.ProblemDetails.Title = errorFeature.Title;
        ctx.ProblemDetails.Detail = errorFeature.Detail;
        ctx.ProblemDetails.Type = errorFeature.ErrorType.ToString();
      }
    });
```

With this file to define Server\Errors\ErrorFeature

```
namespace ErrorDetailsRFC7807.Server.Errors;

public record ErrorFeature(ErrorType ErrorType, string? Title = null, string? Detail = null);

public enum ErrorType
{
    ArgumentException,
    InvalidOperationException,
    NotFoundException
}
```
