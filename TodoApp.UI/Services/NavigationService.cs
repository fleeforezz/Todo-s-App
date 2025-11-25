using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.UI.ViewModels;

namespace TodoApp.UI.Services
{
    public class NavigationService
    {
        public Action<BaseViewModel> _navigate;
        public event Action<BaseViewModel> OnNavigate
        {
            add { _navigate += value; }
            remove { _navigate -= value; }
        }

        public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel, new()
        {
            var viewModel = new TViewModel();
            _navigate?.Invoke(viewModel);
        }

        public void NavigateTo(BaseViewModel viewModel)
        {
            _navigate?.Invoke(viewModel);
        }
    }
}
