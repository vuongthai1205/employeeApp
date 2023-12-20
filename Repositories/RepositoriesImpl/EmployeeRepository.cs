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

        public int countEmployee()
        {
            try{
                var count = _dbContext.Employees.Count();
                return count;
            }catch(Exception){
                throw;
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

        public IEnumerable<Employee> GetAll(Dictionary<string, string> param)
        {
            var query = _dbContext.Employees.AsQueryable();

            if (param != null && param.ContainsKey("sort"))
            {
                string sortDirection = param["sort"].ToLower();

                switch (sortDirection)
                {
                    case "asc":
                        query = query.Include(e => e.Company).OrderBy(e => e.FullName);
                        break;

                    case "desc":
                        query = query.Include(e => e.Company).OrderByDescending(e => e.FullName);
                        break;

                    // Nếu giá trị không hợp lệ hoặc không được cung cấp, bạn có thể xử lý mặc định ở đây.
                    default:
                        query = query.Include(e => e.Company).OrderBy(e => e.FullName);
                        break;
                }
            }

            if (param != null && param.ContainsKey("page")){
                int p = Int32.Parse(param["page"].ToLower());
                int pageSize =10;

                if (p > 0) {  // Kiểm tra nếu page > 0 thì áp dụng giới hạn và vị trí bắt đầu
                    query = query.Skip((p - 1) * pageSize).Take(pageSize);
                }
            }

            return query.AsNoTracking().Include(e => e.Company).ToList();
        }

        public Employee? GetById(int id)
        {
            return _dbContext.Employees.Include(e => e.Company).Single(e => e.StaffCode == id);
        }

    }
}