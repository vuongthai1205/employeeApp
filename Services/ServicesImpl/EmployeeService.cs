using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeApp.Models;
using EmployeeApp.Repositories;

namespace EmployeeApp.Services.ServicesImpl
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository){
            _employeeRepository = employeeRepository;
        }

        public void AddCompanyToEmployee(int id, int idC)
        {
            _employeeRepository.AddCompanyToEmployee(id, idC);
        }

        public Employee AddOrUpdateEmployee(Employee employee)
        {
            if (employee.StaffCode == 0)
            {
            }
            else
            {
                employee.UpdateAt = DateTime.Now;
            }
            return _employeeRepository.AddOrUpdateEmployee(employee);
        }

        public bool DeleteEmployee(int id)
        {
            return _employeeRepository.DeleteEmployee(id);

        }

        public IEnumerable<Employee> GetAll(Dictionary<string, string> param)
        {
            return _employeeRepository.GetAll(param);
        }

        public Employee? GetById(int id)
        {
            return _employeeRepository.GetById(id);
        }
    }
}