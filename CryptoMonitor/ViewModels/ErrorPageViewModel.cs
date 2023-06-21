using CommunityToolkit.Mvvm.Input;
using CryptoMonitor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMonitor.ViewModels
{
    public class ErrorPageViewModel : ViewModelBase
    {
        public ErrorPageViewModel(ErrorService errorService, INavigationService navigationService)
        {
            ErrorService = errorService;
			NavigationService = navigationService;
        }
        private string errorMessage;
		public string ErrorMessage
		{
			get
			{
				return ErrorService.ErrorMessage;
			}
		}

        public ErrorService ErrorService { get; }
        public INavigationService NavigationService { get; }

        private RelayCommand _navigateHome;

		public RelayCommand NavigateHome
		{
			get => _navigateHome ?? (_navigateHome = new RelayCommand(()=>NavigationService.NavigateTo<HomePageViewModel>()));
		}

	}
}
