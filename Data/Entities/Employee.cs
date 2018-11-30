
namespace Data.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int Salary { get; set; }
        public Company Company { get; set; }
    }
}
