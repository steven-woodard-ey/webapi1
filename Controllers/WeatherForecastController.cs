using app1.Data;
using app1.Models;
using Microsoft.AspNetCore.Mvc;

namespace app1.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(
        AppDbContext db,
        ILogger<WeatherForecastController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<EmployeeModel> Get()
    {
        _logger.LogDebug("Getting employees from DB");
        var employees = _db.Employees;
        return employees.ToArray();
    }
}
