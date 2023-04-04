using ErrorDetailsRFC7807.Server.Errors;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseWebAssemblyDebugging();
}
else
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
