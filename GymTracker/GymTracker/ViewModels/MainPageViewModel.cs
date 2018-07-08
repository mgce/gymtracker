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
        public DelegateCommand ShowAddingFormCommand { get; }
        public DelegateCommand<Training> GoToStagePageCommand { get; }
        public DelegateCommand AddTrainingCommand { get; }
        public DelegateCommand<Training> StartTrainingCommand { get; }

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
            ShowAddingFormCommand = new DelegateCommand(async () => await ShowAddingForm());
            GoToStagePageCommand = new DelegateCommand<Training>(async (t) => await GoToStagePage(t));
            AddTrainingCommand = new DelegateCommand(async () => await AddTraining());
            StartTrainingCommand = new DelegateCommand<Training>(async (t) => await StartTraining(t));
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
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

        private bool _addingFormVisible;
        public bool AddingFormVisible
        {
            get => _addingFormVisible;
            set => SetProperty(ref _addingFormVisible, value);
        }

        private async Task StartTraining(Training training)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add(Constants.Models.Training, training);
            await NavigationService.NavigateAsync(nameof(ActiveTrainingPage), navigationParams);
        }

        private async Task GetTrainings()
        {
            await _trainingRepository.GetItemsAsync().ContinueWith(async result =>
            {
                var trainings = await result;
                foreach (var t in trainings)
                {
                    Trainings.Add(t);
                }
            });
        }

        private async Task AddTraining()
        {
            try
            {
                var training = new Training(Name);
                await _trainingRepository.SaveItemAsync(training);
                Trainings.Add(training);
                AddingFormVisible = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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

        private Task ShowAddingForm() => Task.FromResult(AddingFormVisible = true);
    }
}
