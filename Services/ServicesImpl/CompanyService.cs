using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeApp.Models;
using EmployeeApp.Repositories;

namespace EmployeeApp.Services.ServicesImpl
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanyService(ICompanyRepository companyRepository){
            _companyRepository = companyRepository;
        }

        public void AddEmployeesToCompany(int id, int[] employeeIds)
        {
            _companyRepository.AddEmployeesToCompany(id,employeeIds);
        }

        public Company? AddOrUpdateCompany(Company company)
        {
            if (company.Id !=0){
                company.UpdateAt = DateTime.Now;
            }
            return _companyRepository.AddOrUpdateCompany(company);
        }

        public bool DeleteCompany(int id)
        {
            return _companyRepository.DeleteCompany(id);
        }

        public IEnumerable<Company> GetCompanies()
        {
            return _companyRepository.GetCompanies();
        }

        public Company? GetCompanyById(int id)
        {
            return _companyRepository.GetCompanyById(id);
        }
    }
}