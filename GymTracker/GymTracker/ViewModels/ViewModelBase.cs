using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        public DelegateCommand<string> NavigateToCommand { get; }
        public DelegateCommand<string> PushNavigationPopUpCommand { get; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigateToCommand = new DelegateCommand<string>(async (path) => await NavigateTo(path));
            PushNavigationPopUpCommand = new DelegateCommand<string>(async (path) => await PushNavigationPopUp(path));
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
            
        }

        public virtual void Destroy()
        {
            
        }

        public async Task NavigateTo (string path) => await NavigationService.NavigateAsync(path);
        public async Task PushNavigationPopUp (string path) => await NavigationService.PushPopupPageAsync(path);
    }
}
