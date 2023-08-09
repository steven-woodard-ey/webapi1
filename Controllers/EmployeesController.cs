using app1.Data;
using app1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace app1.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(
        AppDbContext db,
        ILogger<EmployeesController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<EmployeeModel> Get()
    {
        return _db.Employees;
    }

    [HttpGet("{id}")]
    public EmployeeModel? GetById(int id)
    {
        return _db.Employees.SingleOrDefault(x => x.EmployeeId == id);
    }

    [HttpPost]
    public async Task<EmployeeModel> Post(EmployeeModel employee)
    {
        employee.EmployeeId = null; // set to null for auto-PK
        var response = _db.Add(employee);
        await _db.SaveChangesAsync();
        return response.Entity;
    }

    [HttpPut]
    public async Task<EmployeeModel?> Put(EmployeeModel employee)
    {
        try
        {
            var response = _db.Update(employee);
            await _db.SaveChangesAsync();
            return response.Entity;
        }
        catch (DbUpdateConcurrencyException)
        {
            return null;
        }
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        var model = GetById(id);
        if (model == null)
        {
            return;
        }
        _db.Remove(model);
        await _db.SaveChangesAsync();
    }
}
