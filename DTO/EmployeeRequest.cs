using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApp.DTO
{
    public class EmployeeRequest
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "LastName is required")]
        public required string LastName { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "FirstName is required")]
        public string? FirstName { get; set; }
        [MaxLength(13)]
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$")]
        public string? Phone { get; set; }
    }
}