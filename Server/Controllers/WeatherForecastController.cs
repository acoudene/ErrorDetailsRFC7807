using ErrorDetailsRFC7807.Server.Errors;
using ErrorDetailsRFC7807.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ErrorDetailsRFC7807.Server.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
      _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
      return Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
      })
      .ToArray();
    }

    [HttpGet]
    [Route("SimpleException")]
    public IActionResult GetSimpleException()
    {
      throw new InvalidOperationException($"Problem while using {nameof(GetSimpleException)} operation");
    }

    [HttpGet]
    [Route("ErrorFeatureOnIActionResult")]
    public IActionResult GetErrorFeatureOnIActionResults()
    {
      var errorType = new ErrorFeature(ErrorType: ErrorType.InvalidOperationException, Detail: $"Problem was found in {nameof(GetErrorFeatureOnIActionResults)}");
      HttpContext.Features.Set(errorType);
      return BadRequest();
    }

    [HttpGet]
    [Route("ErrorFeatureOnResults")]
    public Results<BadRequest, Ok<IEnumerable<WeatherForecast>>> GetErrorFeatureOnResults()
    {
      var errorType = new ErrorFeature(ErrorType: ErrorType.InvalidOperationException, Detail: $"Problem was found in {nameof(GetErrorFeatureOnResults)}");
      HttpContext.Features.Set(errorType);
      return TypedResults.BadRequest();
    }

    [HttpGet]
    [Route("ErrorFeatureOnResultsUsingTypedResultsProblem")]
    public Results<ProblemHttpResult, Ok<IEnumerable<WeatherForecast>>> GetErrorFeatureOnResultsUsingTypedResultsProblem()
    {
      return TypedResults.Problem($"Problem was found in {nameof(GetErrorFeatureOnResultsUsingTypedResultsProblem)}");
    }
  }
}