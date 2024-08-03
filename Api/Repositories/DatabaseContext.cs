using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }

        // Suppressed warnings by setting to "null!"  In .NET 7+, I would use the `required` keyword instead 
        public DbSet<Dependent> Dependents { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ASSUMPTION: Since the below list of employees/dependents were found in the EmployeeController originally,
            // we want that content to be stored in our tables for this demo, so seed the database accordingly.
            List<Employee> defaultEmployees = new()
            {
                new()
                {
                    Id = 1,
                    FirstName = "LeBron",
                    LastName = "James",
                    Salary = 75420.99m,
                    DateOfBirth = new DateTime(1984, 12, 30),
                },
                new()
                {
                    Id = 2,
                    FirstName = "Ja",
                    LastName = "Morant",
                    Salary = 92365.22m,
                    DateOfBirth = new DateTime(1999, 8, 10),
                },
                new()
                {
                    Id = 3,
                    FirstName = "Michael",
                    LastName = "Jordan",
                    Salary = 143211.12m,
                    DateOfBirth = new DateTime(1963, 2, 17),
                }
            };

            modelBuilder.Entity<Employee>()
                .HasData(defaultEmployees);

            List<Dependent> defaultDependents = new()
            {
                new()
                {
                    Id = 1,
                    EmployeeId = 2,
                    FirstName = "Spouse",
                    LastName = "Morant",
                    Relationship = Relationship.Spouse,
                    DateOfBirth = new DateTime(1998, 3, 3),
                },
                new()
                {
                    Id = 2,
                    EmployeeId = 2,
                    FirstName = "Child1",
                    LastName = "Morant",
                    Relationship = Relationship.Child,
                    DateOfBirth = new DateTime(2020, 6, 23)
                },
                new()
                {
                    Id = 3,
                    EmployeeId = 2,
                    FirstName = "Child2",
                    LastName = "Morant",
                    Relationship = Relationship.Child,
                    DateOfBirth = new DateTime(2021, 5, 18)
                },
                new()
                {
                    Id = 4,
                    EmployeeId = 3,
                    FirstName = "DP",
                    LastName = "Jordan",
                    Relationship = Relationship.DomesticPartner,
                    DateOfBirth = new DateTime(1974, 1, 2)
                }
            };

            modelBuilder.Entity<Dependent>()
                .HasData(defaultDependents);
        }
    }
}
