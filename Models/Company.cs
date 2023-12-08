using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Models
{
    [Table("Companies")]
    public class Company
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id{get;set;}
        [Required]
        [MaxLength(50)]
        [Comment("This is last name of company")]
        public string? Name{get;set;}
        [MaxLength(250)]
        [Comment("This is address of company")]
        public string? Address{get;set;}
        public DateTime CreateAt{get;set;}
        public DateTime UpdateAt{get;set;}
        public ICollection<Employee> Employees {get;} = new List<Employee>();
    }
}