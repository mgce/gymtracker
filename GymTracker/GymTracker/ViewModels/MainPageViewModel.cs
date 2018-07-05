using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Helpers;
using GymTracker.Models;
using GymTracker.Repositories;
using GymTracker.Services;
using GymTracker.Views;
using Prism.Services;

namespace GymTracker.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;
        private readonly ITrainingRepository _trainingRepository;
        public DelegateCommand<Training> GoToStagePageCommand { get; }

        public MainPageViewModel(INavigationService navigationService, 
            IPageDialogService dialogService, 
            ITrainingRepository trainingRepository) 
            : base (navigationService)
        {
            _dialogService = dialogService;
            _trainingRepository = trainingRepository;
            Trainings = new ObservableCollection<Training>();
            Title = "Gym tracker";
            Task.Run(async () => await GetTrainings());
            GoToStagePageCommand = new DelegateCommand<Training>(async (t) => await GoToStagePage(t));
        }

        private ObservableCollection<Training> _trainings;
        public ObservableCollection<Training> Trainings
        {
            get => _trainings;
            set
            {
                _trainings = value;
                RaisePropertyChanged();
            }
        }

        private async Task GetTrainings()
        {
            await _trainingRepository.GetItemsAsync().ContinueWith(async r =>
            {
                var trainings = await r;
                foreach (var t in trainings)
                {
                    Trainings.Add(t);
                }
            });
        }

        private async Task GoToStagePage(Training training)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add(Constants.Models.Training, training);
            await NavigationService.NavigateAsync(nameof(StagesPage), navigationParams);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey(Constants.Models.NewTraining))
            {
                var newTraining = parameters.GetValue<Training>(Constants.Models.NewTraining);
                Trainings.Add(newTraining);
            }
        }
    }
}
