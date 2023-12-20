using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Models
{
    [Table("Employees")]
    [PrimaryKey(nameof(StaffCode))]
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("This is primary key")]
        public int StaffCode { get; set; }
        [Comment("This is full name iclude(last name + first name) of employee")]
        [Column(TypeName = "varchar(200)")]
        public string? FullName { get; set; }

        [MaxLength(50)]
        [Comment("This is last name of employee")]
        public string? LastName { get; set; }

        [MaxLength(50)]
        [Comment("This is first name of employee")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [MaxLength(13)]
        [Range(1,13)]
        [Comment("This is phone of employee")]
        [RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$")]
        public string? Phone { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public Company? Company { get; set; }

    }
}