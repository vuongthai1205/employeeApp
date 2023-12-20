using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApp.Models
{
    public class CustomError
    {
        public int Id { get; set; }
        public required string Values { get; set; }
        public DateTime Created { get; set; }
    }
}