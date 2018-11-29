using System.Collections.Generic;
using System.IO;
using EfCore.Entities;
using EfCore.Interfaces;
using Newtonsoft.Json;

namespace EfCore.Implementations
{
    class FileDataContext : IDataProvider
    {
        public Employee AddEmployee(Employee employee)
        {
            employee.EmployeeId = GetNextId();
            var employees = LoadEmployees();
            employees.Add(employee);
            SaveEmployees(employees);
            return employee;
        }

        public bool DeleteEmployee(int id)
        {
            var employees = LoadEmployees();
            employees.Remove(employees.Find(e => e.EmployeeId == id));
            SaveEmployees(employees);
            return true;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return LoadEmployees();
        }

        private List<Employee> LoadEmployees()
        {
            var path = Directory.GetCurrentDirectory()+ "/emp";

            if (!File.Exists(path))
                File.WriteAllText(path, "");
            var employees = JsonConvert.DeserializeObject<List<Employee>>(File.ReadAllText(path));
            return employees ?? new List<Employee>();
        }

        private void SaveEmployees(List<Employee> employees)
        {
            var path = Directory.GetCurrentDirectory()+"/emp";
            File.WriteAllText(path, JsonConvert.SerializeObject(employees));
        }

        private int GetNextId()
        {
            var path = Directory.GetCurrentDirectory()+"/id";
            if (!File.Exists(path))
                File.WriteAllText(path, 1.ToString());

            var id = int.Parse(File.ReadAllText(path));
            File.WriteAllText(path, (id+1).ToString());
            return id;
        }
    }
}
