using EfCore.Entities;
using System.Collections.Generic;

namespace EfCore.Interfaces
{
    interface IDataProvider
    {
        IEnumerable<Employee> GetAllEmployees();
        Employee AddEmployee(Employee employee);
        bool DeleteEmployee(int id);
    }
}
