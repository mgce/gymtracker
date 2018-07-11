using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Helpers;
using GymTracker.Models;
using GymTracker.Repositories;
using GymTracker.Services;
using GymTracker.Views;
using Prism.Commands;
using Prism.Navigation;

namespace GymTracker.ViewModels
{
    public class StagesViewModel : ViewModelBase
    {
        private readonly IStageTemplateRepository _stageTemplateRepository;
        public DelegateCommand ShowAddingFormCommand { get;}
        public DelegateCommand AddStageCommand { get;}
        public DelegateCommand<StageTemplate> NavigateToExercisesPageCommand { get;}

        public StagesViewModel(INavigationService navigationService, IStageTemplateRepository stageTemplateRepository)
            : base(navigationService)
        {
            _stageTemplateRepository = stageTemplateRepository;
            ShowAddingFormCommand = new DelegateCommand(async () => await ShowAddingForm());
            NavigateToExercisesPageCommand = new DelegateCommand<StageTemplate>(async (s) => await NavigateToExercisesPage(s));
            AddStageCommand = new DelegateCommand(async () => await AddStage());
            Stages = new ObservableCollection<StageTemplate>();
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

        private TrainingTemplate _training;
        public TrainingTemplate Training
        {
            get => _training;
            set => SetProperty(ref _training, value);
        }

        private ObservableCollection<StageTemplate> _stages;
        public ObservableCollection<StageTemplate> Stages
        {
            get => _stages;
            set => SetProperty(ref _stages, value);
        }

        public async Task AddStage()
        {
            if (!string.IsNullOrEmpty(NewStageName))
            {
                var stage = new StageTemplate(NewStageName, Training.Id);
                await _stageTemplateRepository.SaveItemAsync(stage);
                Stages.Add(stage);
                AddingFormVisible = false;
                NewStageName = String.Empty;
            }
        }

        public async Task NavigateToExercisesPage(StageTemplate stage)
        {
            var navigationParams = new NavigationParameters
            {
                {Constants.Models.Stage, stage}
            };
            await NavigationService.NavigateAsync(nameof(ExercisesPage), navigationParams);
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey(Constants.Models.Training))
            {
                Training = parameters.GetValue<TrainingTemplate>(Constants.Models.Training);
                var stages = await _stageTemplateRepository.GetStagesByTrainingTemplateId(Training.Id);
                if(stages != null)
                    Stages.AddRange(stages);
            }

            if (parameters.ContainsKey(Constants.Models.NewStage))
            {
                Stages.AddRange(await _stageTemplateRepository.GetStagesByTrainingTemplateId(Training.Id));
            }
        }

        private Task ShowAddingForm() => Task.FromResult(AddingFormVisible = true);
    }
}
