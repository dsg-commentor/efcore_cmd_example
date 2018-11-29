using System;
using System.IO;
using EfCore.Entities;
using EfCore.Implementations;
using EfCore.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EfCore
{
    class Program
    {
        static bool StopRunning;
        static ServiceProvider _services;
        static void Main(string[] args)
        {
            var diContainer = new ServiceCollection();
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            diContainer.AddSingleton(_ => builder.Build());

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-provider")
                {
                    var provider = args[i + 1];

                    if (provider == "efcore")
                    {
                        diContainer.AddDbContext<EfDataContext>();
                        diContainer.AddSingleton<IDataProvider>(new EfDataContext(builder.Build().GetConnectionString("sql")));
                    }

                    else if (provider == "file")
                        diContainer.AddSingleton<IDataProvider, FileDataContext>();
                }
            }
            _services = diContainer.BuildServiceProvider();

            Console.WriteLine("Example App for demonstrating Ef Core");
            Help();
            do MainLoop();
            while (!StopRunning);
        }


        private static void MainLoop()
        {
            Console.WriteLine("Please select an action");
            try
            {
                char selection = Console.ReadKey().KeyChar.ToString().ToLower()[0];
                Console.WriteLine();
                switch (selection)
                {
                    case 'g':
                        Get();
                        break;
                    case 'c':
                        Create();
                        break;
                    case 'd':
                        Delete();
                        break;
                    case 'q':
                        Quit();
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error handling input");
            }
        }

        private static void Get()
        {
            var employees =_services.GetService<IDataProvider>().GetAllEmployees();
            foreach (var employee in employees)
            {
                Console.WriteLine($"Id: {employee.EmployeeId}, Name: {employee.Name}, Title: {employee.Title}, Salary: {employee.Salary}");
            }
        }

        private static void Create()
        {
            try
            {
                Console.WriteLine("Name:");
                var name = Console.ReadLine();
                Console.WriteLine("Company Id:");
                var companyId = Console.ReadLine();
                Console.WriteLine("Title:");
                var title = Console.ReadLine();
                Console.WriteLine("Salary $:");
                var salary = Console.ReadLine();

                _services.GetService<IDataProvider>().AddEmployee(new Employee { Name = name, Title = title, Salary = int.Parse(salary)});
            }
            catch (Exception)
            {
                Console.WriteLine("Error handling input:");
                MainLoop();
            }
        }

        private static void Delete()
        {
            Console.WriteLine("Please type the Id of the Employee to be deleted");
            var id = Console.ReadLine();
            _services.GetService<IDataProvider>().DeleteEmployee(int.Parse(id));
        }

        private static void Quit()
        {
            StopRunning = true;
            Console.WriteLine("Press any key to close window");
            Console.ReadKey();
        }

        private static void Help()
        {
            Console.WriteLine("Options are as follows:");
            Console.WriteLine("(G)et");
            Console.WriteLine("(C)reate");
            Console.WriteLine("(D)elete");
            Console.WriteLine("(Q)uit");
        }
    }
}
