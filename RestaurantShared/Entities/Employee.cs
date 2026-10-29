using System;
using System.Collections.Generic;

namespace RestaurantDataAccess.Entities;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public int UserId { get; set; }

    public int? ManagerId { get; set; }

    public decimal? Salary { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

  
    public virtual ICollection<Employee> InverseManager { get; set; } = new List<Employee>();

    public virtual Employee? Manager { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
