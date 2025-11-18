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
        public Action<BaseViewModel> Navigate;

        public void NavigateTo(BaseViewModel viewModel)
        {
            Navigate?.Invoke(viewModel);
        }
    }
}
