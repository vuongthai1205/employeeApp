using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Configs
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
        : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().Property(e => e.CreateAt).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Employee>().Property(e => e.UpdateAt).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Company>().Property(e => e.UpdateAt).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Company>().Property(e => e.CreateAt).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Employee>()
                        .Property(e => e.FullName)
                        .HasComputedColumnSql("[LastName] + ' ' + [FirstName]", stored: true);
            modelBuilder.Entity<Employee>().HasIndex(e => e.Phone).IsUnique();
            modelBuilder.Entity<Company>().HasIndex(e => e.Name).IsUnique();
        }
    }
}