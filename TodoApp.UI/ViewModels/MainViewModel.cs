using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoApp.BLL.Services;
using TodoApp.DAL;
using TodoApp.DAL.Repositories;
using TodoApp.UI.Command;
using TodoApp.UI.Services;

namespace TodoApp.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;
        private readonly NavigationService _navService;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand NavigateToLoginCommand { get; }
        public ICommand NavigateToSignUpCommand { get; }
        public ICommand NavigateToTodoViewCommand { get; }

        public MainViewModel(NavigationService navigationService, LoginViewModel loginViewModel, SignUpViewModel signUpViewModel)
        {
            _navService = navigationService;
            // Subscribe to navigation events
            _navService.OnNavigate += (vm) => CurrentViewModel = vm;

            var db = new TodoDbContext();
            var userRepo = new UserRepository(db);
            var userService = new UserService(userRepo);

            CurrentViewModel = loginViewModel;

            // Commands to navigate between views
            NavigateToLoginCommand = new RelayCommand(o => _navService.NavigateTo(loginViewModel));
            NavigateToSignUpCommand = new RelayCommand(o => _navService.NavigateTo(signUpViewModel));
            NavigateToTodoViewCommand = new RelayCommand(o => 
            {
                var todoRepo = new TodoRepository(db);
                var tagRepo = new TagRepository(db);
                var todoService = new TodoService(todoRepo);
                var tagService = new TagService(tagRepo);
                var todoViewModel = new TodoViewModel(_navService, userService, todoService, tagService);
                _navService.NavigateTo(todoViewModel);
            });
        }
    }
}
