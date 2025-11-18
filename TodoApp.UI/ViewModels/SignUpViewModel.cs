using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TodoApp.BLL.Services;
using TodoApp.DAL.Entities;
using TodoApp.UI.Command;
using TodoApp.UI.Services;

namespace TodoApp.UI.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        private readonly NavigationService _nav;
        private readonly UserService _userService;

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RelayCommand RegisterCommand { get; set; }
        public RelayCommand GoToLoginCommand { get; set; }

        public SignUpViewModel(NavigationService nav, UserService userService)
        {
            _nav = nav;
            _userService = userService;

            RegisterCommand = new RelayCommand(o => Register());
            GoToLoginCommand = new RelayCommand(o => _nav.NavigateTo(new LoginViewModel(_nav, _userService)));
        }

        private void Register()
        {
            if (string.IsNullOrWhiteSpace(Username) ||
                    string.IsNullOrWhiteSpace(Email) ||
                    string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Username = Username,
                Email = Email,
                PasswordHash = Password
            };

            _userService.SignUp(user);

            MessageBox.Show("Account created!");

            _nav.NavigateTo(new LoginViewModel(_nav, _userService));
        }
    }
}
