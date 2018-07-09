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
using Xamarin.Forms;

namespace GymTracker.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;
        private readonly ITrainingTemplateRepository _trainingTemplateRepository;
        public DelegateCommand ShowAddingFormCommand { get; }
        public DelegateCommand<TrainingTemplate> GoToStagePageCommand { get; }
        public DelegateCommand AddTrainingCommand { get; }
        public DelegateCommand<TrainingTemplate> StartTrainingCommand { get; }

        public MainPageViewModel(INavigationService navigationService, 
            IPageDialogService dialogService, 
            ITrainingTemplateRepository trainingTemplateRepository) 
            : base (navigationService)
        {
            _dialogService = dialogService;
            _trainingTemplateRepository = trainingTemplateRepository;
            Trainings = new ObservableCollection<TrainingTemplate>();
            Title = "Gym tracker";
            Task.Run(async () => await GetTrainings());
            ShowAddingFormCommand = new DelegateCommand(async () => await ShowAddingForm());
            GoToStagePageCommand = new DelegateCommand<TrainingTemplate>(async (t) => await GoToStagePage(t));
            AddTrainingCommand = new DelegateCommand(async () => await AddTraining());
            StartTrainingCommand = new DelegateCommand<TrainingTemplate>(async (t) => await StartTraining(t));
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private ObservableCollection<TrainingTemplate> _trainings;
        public ObservableCollection<TrainingTemplate> Trainings
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

        private async Task StartTraining(TrainingTemplate training)
        {
            var navigationParams = new NavigationParameters
            {
                {Constants.Models.Training, training}
            };
            await NavigationService.NavigateAsync(nameof(ActiveTrainingPage), navigationParams);
        }

        private async Task GetTrainings()
        {
            var trainings = await _trainingTemplateRepository.GetItemsAsync();
            Device.BeginInvokeOnMainThread(() =>
                Trainings.AddRange(trainings)
            );
        }

        private async Task AddTraining()
        {
            try
            {
                var training = new TrainingTemplate(Name);
                await _trainingTemplateRepository.SaveItemAsync(training);
                Trainings.Add(training);
                AddingFormVisible = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task GoToStagePage(TrainingTemplate training)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add(Constants.Models.Training, training);
            await NavigationService.NavigateAsync(nameof(StagesPage), navigationParams);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey(Constants.Models.NewTraining))
            {
                var newTraining = parameters.GetValue<TrainingTemplate>(Constants.Models.NewTraining);
                Trainings.Add(newTraining);
            }
        }

        private Task ShowAddingForm() => Task.FromResult(AddingFormVisible = true);
    }
}
