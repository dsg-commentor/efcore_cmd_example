using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Data.Entities;
using ServiceInterfaces;

namespace EfCoreImpl
{
    public class EfDataContext : DbContext, IDataProvider
    {
        private readonly string _connectionString;

        public EfDataContext(DbContextOptions<EfDataContext> options)
        : base(options)
        { }

        public EfDataContext(string dbConnection)
        {
            _connectionString = dbConnection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return Employees.ToListAsync().GetAwaiter().GetResult();
        }

        public Employee AddEmployee(Employee employee)
        {
            Employees.Add(employee);
            SaveChanges();
            return employee;
        }

        public bool DeleteEmployee(int id)
        {
            Employee e = Employees.Find(id);
            Employees.Remove(e);
            SaveChanges();
            return true;
        }
    }
}
