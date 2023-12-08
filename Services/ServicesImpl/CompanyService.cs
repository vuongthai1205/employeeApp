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

        public void AddEmployeeToCompany(int id, int idE)
        {
            _companyRepository.AddEmployeeToCompany(id,idE);
        }

        public Company? AddOrUpdateCompany(Company company)
        {
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