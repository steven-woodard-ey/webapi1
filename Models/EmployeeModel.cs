using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app1.Models;

[Table("employees")]
public class EmployeeModel
{
    [Key, Column("employee_id")]
    public int EmployeeId { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}
