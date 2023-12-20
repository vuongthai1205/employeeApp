using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeApp.Models;

namespace EmployeeApp.Services
{
    public interface ICompanyService
    {
        public Company? AddOrUpdateCompany(Company company);
        public bool DeleteCompany(int id);
        public Company? GetCompanyById(int id);
        public IEnumerable<Company> GetCompanies();
        public void AddEmployeesToCompany(int id, int[] employeeIds);
    }
}