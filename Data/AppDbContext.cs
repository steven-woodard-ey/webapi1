using app1.Models;
using Microsoft.EntityFrameworkCore;

namespace app1.Data;

public class AppDbContext : DbContext
{
    private static readonly string DB_CONNSTR_CONFIG_NAME = "AppDbContext";
    private static readonly string DB_NAME_ENV_VAR = "DB_NAME";
    private static readonly string DB_USERNAME_ENV_VAR = "DB_USERNAME";
    private static readonly string DB_PASSWORD_ENV_VAR = "DB_PASSWORD";

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

        options.UseNpgsql(GetConnectionString());
    }

    private string GetConnectionString()
    {
        var connStr = Configuration.GetConnectionString(DB_CONNSTR_CONFIG_NAME);
        if (string.IsNullOrEmpty(connStr))
        {
            throw new Exception($"No connection string specified for {DB_CONNSTR_CONFIG_NAME}");
        }
        if (!connStr.EndsWith(";"))
        {
            connStr += ";";
        }
        if (!connStr.Contains("Database="))
        {
            _logger.LogInformation("DB database name not found in config, reading from Environment");
            connStr += $"Database={Environment.GetEnvironmentVariable(DB_NAME_ENV_VAR)};";
        }
        if (!connStr.Contains("Username="))
        {
            _logger.LogInformation("DB username not found in config, reading from Environment");
            connStr += $"Username={Environment.GetEnvironmentVariable(DB_USERNAME_ENV_VAR)};";
        }
        if (!connStr.Contains("Password="))
        {
            _logger.LogInformation("DB password not found in config, reading from Environment");
            connStr += $"Password={Environment.GetEnvironmentVariable(DB_PASSWORD_ENV_VAR)};";
        }
        return connStr;
    }

    public DbSet<EmployeeModel> Employees { get; set; }
}