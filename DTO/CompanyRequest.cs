using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApp.DTO
{
    public class CompanyRequest
    {
        [Required]
        [MaxLength(50)]
        public required string Name{get;set;}
        [MaxLength(250)]
        public string? Address{get;set;}
    }
}