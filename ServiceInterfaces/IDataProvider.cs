using Data.Entities;
using System.Collections.Generic;

namespace ServiceInterfaces
{
    public interface IDataProvider
    {
        IEnumerable<Employee> GetAllEmployees();
        Employee AddEmployee(Employee employee);
        bool DeleteEmployee(int id);
    }
}
