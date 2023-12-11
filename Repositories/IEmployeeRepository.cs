using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeApp.Models;

namespace EmployeeApp.Repositories
{
    public interface IEmployeeRepository
    {
        public Employee AddOrUpdateEmployee(Employee employee);
        public bool DeleteEmployee(int id);
        public IEnumerable<Employee> GetAll(Dictionary<string, string> param);
        public Employee? GetById(int id);
        public void AddCompanyToEmployee(int id, int idC);
    }
}