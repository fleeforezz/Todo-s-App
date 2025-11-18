using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TodoApp.BLL.Services;
using TodoApp.DAL;
using TodoApp.DAL.Repositories;
using TodoApp.UI.Command;
using TodoApp.UI.Services;

namespace TodoApp.UI.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly NavigationService _nav;
        private readonly UserService _userService;
        private readonly TodoService _todoService;
        private readonly CategoryService _categoryService;

        private string _email;
        public string Email
        {
            get => _email;
            set 
            { 
                _email = value; 
                OnPropertyChanged(); 
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set 
            { 
                _password = value; 
                OnPropertyChanged(); 
            }
        }

        public LoginViewModel(NavigationService nav, UserService userService)
        {
            _nav = nav;
            _userService = userService;

            // Initialize the services
            var db = new TodoDbContext();
            var todoRepo = new TodoRepository(db);
            var categoryRepo = new CategoryRepository(db);

            _todoService = new TodoService(todoRepo);
            _categoryService = new CategoryService(categoryRepo);

            LoginCommand = new RelayCommand(o => Login());
            GoToSignUpCommand = new RelayCommand(o => _nav.NavigateTo(new SignUpViewModel(_nav, _userService)));

        }

        #region Commands
        public RelayCommand LoginCommand { get; set; }
        public RelayCommand GoToSignUpCommand { get; set; }
        #endregion

        #region Properties
        private void Login()
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Please enter both email and password.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            try
            {
                var user = _userService.Login(Email, Password);
                if (user != null)
                {
                    _nav.NavigateTo(new TodoViewModel(_nav, user, _todoService, _categoryService));
                }
                else
                {
                    MessageBox.Show("Invalid email or password", "Login Failed",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
