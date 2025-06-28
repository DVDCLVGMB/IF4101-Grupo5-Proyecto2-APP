using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management_App.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public int DeptId { get; set; }
        public int RoleId { get; set; }
        public string Extension { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public string WorkPhoneNumber { get; set; }

        public string? DepartmentName { get; set; }
        public string? RoleName { get; set; }

    }
}
