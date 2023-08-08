using app1.Models;
using Microsoft.EntityFrameworkCore;

namespace app1.Data;

public class AppDbContext : DbContext
{
    private readonly ILogger<AppDbContext> _logger;
    protected readonly IConfiguration Configuration;

    public AppDbContext(ILogger<AppDbContext> logger, IConfiguration configuration)
    {
        _logger = logger;
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings

        options.UseNpgsql(GetConnectionString("AppDbContext", "DB_USERNAME", "DB_PASSWORD"));
    }

    private string GetConnectionString(string configName, string envVarUsername, string envVarPassword)
    {
        var connStr = Configuration.GetConnectionString(configName);
        if (string.IsNullOrEmpty(connStr))
        {
            throw new Exception($"No connection string specified for {configName}");
        }
        if (!connStr.EndsWith(";"))
        {
            connStr += ";";
        }
        if (!connStr.Contains("Username="))
        {
            _logger.LogInformation("DB username not found in config, reading from Environment");
            connStr += $"Username={Environment.GetEnvironmentVariable(envVarUsername)};";
        }
        if (!connStr.Contains("Password="))
        {
            _logger.LogInformation("DB password not found in config, reading from Environment");
            connStr += $"Password={Environment.GetEnvironmentVariable(envVarPassword)};";
        }
        return connStr;
    }

    public DbSet<EmployeeModel> Employees { get; set; }
}