﻿using System.Collections.Generic;

namespace EfCore.Entities
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public int YearFounded { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
    }
}
