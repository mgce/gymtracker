using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Helpers;
using GymTracker.Models;
using GymTracker.Repositories;
using GymTracker.Services;
using Prism.Commands;
using Prism.Navigation;

namespace GymTracker.ViewModels
{
    public class StagesViewModel : ViewModelBase
    {
        private readonly IStageRepository _stageRepository;
        public DelegateCommand ShowAddingFormCommand { get;}
        public DelegateCommand AddStageCommand { get;}

        public StagesViewModel(INavigationService navigationService, IStageRepository stageRepository)
            : base(navigationService)
        {
            _stageRepository = stageRepository;
            ShowAddingFormCommand = new DelegateCommand(async () => await ShowAddingForm());
            AddStageCommand = new DelegateCommand(async () => await AddStage());
            Stages = new ObservableCollection<Stage>();
        }

        private bool _addingFormVisible;
        public bool AddingFormVisible
        {
            get => _addingFormVisible;
            set => SetProperty(ref _addingFormVisible, value);
        }

        private string _newStageName;
        public string NewStageName
        {
            get => _newStageName;
            set => SetProperty(ref _newStageName, value);
        }

        private Training _training;
        public Training Training
        {
            get => _training;
            set => SetProperty(ref _training, value);
        }

        private ObservableCollection<Stage> _stages;
        public ObservableCollection<Stage> Stages
        {
            get => _stages;
            set => SetProperty(ref _stages, value);
        }

        public async Task AddStage()
        {
            if (!string.IsNullOrEmpty(NewStageName))
            {
                var stage = new Stage(NewStageName, Training.Id);
                await _stageRepository.SaveItemAsync(stage);
                Stages.Add(stage);
            }
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey(Constants.Models.Training))
            {
                Training = parameters.GetValue<Training>(Constants.Models.Training);
                var stages = await _stageRepository.GetStagesByTrainingId(Training.Id);
                if(stages != null)
                    Stages.AddRange(stages);
            }

            if (parameters.ContainsKey(Constants.Models.NewStage))
            {
                Stages.AddRange(await _stageRepository.GetStagesByTrainingId(Training.Id));
            }
        }

        private Task ShowAddingForm() => Task.FromResult(AddingFormVisible = true);
    }
}
