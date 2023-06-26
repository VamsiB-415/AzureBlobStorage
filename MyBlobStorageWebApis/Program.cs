using MyBlobStorageWebApis.Common;
using MyBlobStorageWebApis.Repository;
using MyBlobStorageWebApis.Services;
using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

StaticLogger.EnsureInitialized();
Log.Information("Azure Storage API Booting Up...");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add Azure Repository Service
    builder.Services.AddTransient<IAzureStorage, AzureStorage>();
    Log.Information("Services has been successfully added...");

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
    Log.Information("API is now ready to serve files to and from Azure Cloud Storage...");
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled Exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Azure Storage API Shutting Down...");
    Log.CloseAndFlush();
}