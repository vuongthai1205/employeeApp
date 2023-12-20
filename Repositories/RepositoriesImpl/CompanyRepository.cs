using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeApp.Configs;
using EmployeeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Repositories.RepositoriesImpl
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly EmployeeDbContext _employeeDbContext;
        public CompanyRepository(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        public void AddEmployeesToCompany(int id, int[] employeeIds)
        {
            using var transaction = _employeeDbContext.Database.BeginTransaction();

            try
            {
                var company = _employeeDbContext.Companies.Find(id);

                if (company != null)
                {
                    foreach (var employeeId in employeeIds)
                    {
                        var employee = _employeeDbContext.Employees.Find(employeeId);

                        if (employee != null)
                        {
                            company.Employees.Add(employee);
                        }
                    }

                    _employeeDbContext.Companies.Update(company);
                    _employeeDbContext.SaveChanges();
                    transaction.Commit();
                }
                else
                {
                    throw new InvalidOperationException("Company not found");
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                // Handle or log the exception as needed
                throw;
            }
        }



        public Company? AddOrUpdateCompany(Company company)
        {
            using var transaction = _employeeDbContext.Database.BeginTransaction();
            try
            {
                if (company.Id == 0)
                {
                    _employeeDbContext.Companies.Add(company);
                    _employeeDbContext.SaveChanges();
                    transaction.Commit();
                    return company;
                }
                else
                {
                    _employeeDbContext.Companies.Update(company);
                    _employeeDbContext.SaveChanges();
                    transaction.Commit();
                    return company;
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }

        public bool DeleteCompany(int id)
        {
            using var transaction = _employeeDbContext.Database.BeginTransaction();
            try
            {
                var company = _employeeDbContext.Companies.Include(c => c.Employees).Single(c => c.Id == id);
                if (company is not null)
                {
                    foreach (var employee in company.Employees)
                    {
                        employee.Company = null;
                    }
                    _employeeDbContext.Companies.Remove(company);
                    _employeeDbContext.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }

        public IEnumerable<Company> GetCompanies()
        {

            return _employeeDbContext.Companies.ToList();
        }

        public Company? GetCompanyById(int id)
        {

            return _employeeDbContext.Companies.Include(c => c.Employees).AsNoTracking().SingleOrDefault(c => c.Id == id);
        }
    }
}