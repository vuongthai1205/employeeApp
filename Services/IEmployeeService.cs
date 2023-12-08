using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeApp.Models;

namespace EmployeeApp.Services
{
    public interface IEmployeeService
    {
        public Employee AddOrUpdateEmployee(Employee employee);
        public bool DeleteEmployee(int id);
        public IEnumerable<Employee> GetAll();
        public Employee? GetById(int id);
        public void AddCompanyToEmployee(int id, int idC);
    }
}