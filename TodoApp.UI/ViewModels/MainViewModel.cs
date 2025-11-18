using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.BLL.Services;
using TodoApp.DAL;
using TodoApp.DAL.Repositories;
using TodoApp.UI.Services;

namespace TodoApp.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public NavigationService NavService { get; set; }

        public MainViewModel()
        {
            NavService = new NavigationService();
            NavService.Navigate = (vm) => CurrentViewModel = vm;

            var db = new TodoDbContext();
            var userRepo = new UserRepository(db);
            var userService = new UserService(userRepo);

            CurrentViewModel = new LoginViewModel(NavService, userService);
        }
    }
}
