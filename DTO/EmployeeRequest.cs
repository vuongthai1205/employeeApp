using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApp.DTO
{
    public class EmployeeRequest
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Phone { get; set; }
    }
}