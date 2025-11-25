using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using TodoApp.BLL.Interfaces;
using TodoApp.BLL.Services;
using TodoApp.DAL;
using TodoApp.DAL.Entities;
using TodoApp.DAL.Repositories;
using TodoApp.UI.Services;
using TodoApp.UI.ViewModels;
using TodoApp.UI.Views;

namespace TodoApp.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", true, true)
                       .Build();
            var strConn = config["ConnectionStrings:DefaultConnection"];

            return strConn;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigurationServices(serviceCollection);
        }

        private void ConfigurationServices(IServiceCollection services)
        {
            // DbContext
            services.AddDbContext<TodoDbContext>(options =>
            {
                options.UseSqlServer(GetConnectionString());
            });

            // Repositories - DAL
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<Todo>, TodoRepository>();
            services.AddScoped<IRepository<Tag>, TagRepository>();

            // Services - BLL
            services.AddScoped<ITodoService, TodoService>();
            //services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITagService, TagService>();

            // Register ViewModels - UI
            services.AddTransient<BaseViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<TodoViewModel>();

            // Register Views - UI
            services.AddTransient<MainView>();

            // Register Services - UI
            services.AddSingleton<NavigationService>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ServiceProvider?.Dispose();
            base.OnExit(e);
        }
    }

}
