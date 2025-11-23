using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Infrastructure.Persistences;

namespace Todo.Infrastructure.Data
{
    public class TodoDbContextFactory : IDesignTimeDbContextFactory<TodoDbContext>
    {
        public TodoDbContext CreateDbContext(string[] args)
        {
            // Option 1: Read from WinUI project's appsettings.json
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Todo.WinUI");

            // If running migrations from the Package Manager Console, try both paths
            if (!Directory.Exists(basePath))
            {
                basePath = Path.Combine(Directory.GetCurrentDirectory(), "../../Todo.WinUI");
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TodoDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly("Todo.Infrastructure")
            );

            return new TodoDbContext(optionsBuilder.Options);
        }
    }
}
