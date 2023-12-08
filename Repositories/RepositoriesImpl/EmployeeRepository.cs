using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeApp.Configs;
using EmployeeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Repositories.RepositoriesImpl
{

    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _dbContext;
        public EmployeeRepository(EmployeeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddCompanyToEmployee(int id, int idC)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var e = _dbContext.Employees.Find(id);
                var c = _dbContext.Companies.Find(idC);

                if (e is not null && c is not null)
                {
                    e.Company = c;
                    _dbContext.SaveChanges();
                    transaction.CreateSavepoint("SaveCompanyToEmployee");
                }
                else
                {
                    transaction.RollbackToSavepoint("SaveCompanyToEmployee");
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.RollbackToSavepoint("SaveCompanyToEmployee");
            }
        }

        public Employee AddOrUpdateEmployee(Employee employee)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                if (employee.StaffCode == 0)
                {
                    _dbContext.Employees.Add(employee);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                    return employee;
                }
                else
                {
                    _dbContext.Employees.Update(employee);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                    return employee;
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw new Exception();
            }


        }

        public bool DeleteEmployee(int id)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var employee = _dbContext.Employees.Find(id);
                if (employee is not null)
                {
                    _dbContext.Employees.Remove(employee);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw new Exception();
            }

        }

        public IEnumerable<Employee> GetAll()
        {
            return _dbContext.Employees.AsNoTracking().ToList();
        }

        public Employee? GetById(int id)
        {
            return _dbContext.Employees.Include(e => e.Company).Single(e => e.StaffCode == id);
        }

    }
}