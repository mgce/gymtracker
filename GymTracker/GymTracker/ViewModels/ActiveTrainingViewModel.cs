using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Helpers;
using GymTracker.Models;
using GymTracker.Repositories;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace GymTracker.ViewModels
{
    public class ActiveTrainingViewModel : ViewModelBase
    {
        private readonly IStageTemplateRepository _stageTemplateRepository;
        private readonly IExerciseTemplateRepository _exerciseTemplateRepository;
        private readonly ITrainingRepository _trainingRepository;
        private readonly IStageRepository _stageRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IPageDialogService _dialogService;
        public DelegateCommand GoToNextStageCommand { get; set; }
        public DelegateCommand GoToPreviousStageCommand { get; set; }
        private List<Stage> _stages;
        private List<StageTemplate> _stageTemplates;

        public ActiveTrainingViewModel(INavigationService navigationService, 
            IStageTemplateRepository stageTemplateRepository, 
            IExerciseTemplateRepository exerciseTemplateRepository, 
            ITrainingRepository trainingRepository,
            IStageRepository stageRepository,
            IExerciseRepository exerciseRepository,
            IPageDialogService dialogService) 
            : base(navigationService)
        {
            _stageTemplateRepository = stageTemplateRepository;
            _exerciseTemplateRepository = exerciseTemplateRepository;
            _trainingRepository = trainingRepository;
            _stageRepository = stageRepository;
            _exerciseRepository = exerciseRepository;
            _dialogService = dialogService;
            _stages = new List<Stage>();
            GoToNextStageCommand = new DelegateCommand(async()=>await GoToNextStage());
            GoToPreviousStageCommand = new DelegateCommand(async()=>await GoToPreviousStage());
            GrouppedSets = new ObservableCollection<GrouppedSets>();
        }

        private ObservableCollection<GrouppedSets> _grouppedSets;
        public ObservableCollection<GrouppedSets> GrouppedSets
        {
            get => _grouppedSets;
            set => SetProperty(ref _grouppedSets, value);
        }

        private StageTemplate _currentStage;
        public StageTemplate CurrentStage
        {
            get => _currentStage;
            set => SetProperty(ref _currentStage, value);
        }

        private int _index;
        public int Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }

        private Training _training;
        public Training Training
        {
            get => _training;
            set => SetProperty(ref _training, value);
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey(Constants.Models.Training))
            {
                var trainingTemplate = parameters.GetValue<TrainingTemplate>(Constants.Models.Training);
                await CreateTrainingFromTemplate(trainingTemplate);

                await Task.Run(() => _stageTemplateRepository.GetStagesByTrainingId(Training.Id)).ContinueWith(async stages =>
                       {
                           _stageTemplates = stages.Result;
                           
                           if (_stageTemplates?.Count == 0)
                           {
                               await _dialogService.DisplayAlertAsync("Empty stages",
                                   "Sorry, but you not created any stage yet.", "Go Back");
                               return Task.CompletedTask;
                               //await NavigationService.GoBackAsync();
                           }

                           var firstStageTemplateId = _stageTemplates.First().Id;

                           await CreateStagesFromTemplate(_stageTemplates, Training.Id);
                           CurrentStage = _stageTemplates.FirstOrDefault();
                           Index = _stageTemplates.IndexOf(CurrentStage);

                           var exerciseTemplate = await
                               _exerciseTemplateRepository.GetByStageTemplateId(firstStageTemplateId);
                           var exercises = await CreateExercisesFromTemplate(exerciseTemplate, firstStageTemplateId);
                           var exerciseViewModel = CreateExerciseViewModelsFromExercises(exercises);
                           CreateGrouppedSetsFromExerciseViewModels(exerciseViewModel);

                           return Task.CompletedTask;
                       });
            }
        }
        private async Task<List<Exercise>> CreateExercisesFromTemplate(List<ExerciseTemplate> templates, int stageId)
        {
            var exercisesList = new List<Exercise>();

            foreach (var template in templates)
            {
                var exercise = new Exercise(template, stageId);
                await _exerciseRepository.SaveItemAsync(exercise);
                exercisesList.Add(exercise);
            }

            return exercisesList;
        }

        private List<ExerciseViewModel> CreateExerciseViewModelsFromExercises(List<Exercise> exercises)
        {
            var exerciseViewModels = new List<ExerciseViewModel>();
            foreach (var exercise in exercises)
            {
                exerciseViewModels.Add(new ExerciseViewModel(exercise));
            }

            return exerciseViewModels;
        }

        private void CreateGrouppedSetsFromExerciseViewModels(List<ExerciseViewModel> exerciseViewModels)
        {
            foreach (var exerciseViewModel in exerciseViewModels)
            {
                GrouppedSets.Add(new GrouppedSets(exerciseViewModel));
            }
        }

        private async Task CreateStagesFromTemplate(List<StageTemplate> templates, int trainingId)
        {
            foreach (var template in templates)
            {
                var stage = new Stage(template, trainingId);
                await _stageRepository.SaveItemAsync(stage);
                _stages.Add(stage);
            }
        }

        private async Task CreateTrainingFromTemplate(TrainingTemplate template)
        {
            Training = new Training(template);
            await _trainingRepository.SaveItemAsync(Training);
        }

        private async Task GoToNextStage()
        {
            var lastIndex = _stages.Count - 1;
            var nextIndex = Index + 1;

            if (nextIndex <= lastIndex)
                await SwapStages(nextIndex);
        }

        private async Task GoToPreviousStage()
        {
            var previousIndex = Index - 1;

            if (previousIndex >= 0)
                await SwapStages(previousIndex);
        }

        private async Task SwapStages(int index)
        {
            CurrentStage = _stageTemplates[index];
            GrouppedSets = new ObservableCollection<GrouppedSets>();

            var exerciseTemplates = await _exerciseTemplateRepository.GetByStageTemplateId(CurrentStage.Id);
            var exercise = await CreateExercisesFromTemplate(exerciseTemplates, CurrentStage.Id);
            var exerciseViewModels = CreateExerciseViewModelsFromExercises(exercise);
            CreateGrouppedSetsFromExerciseViewModels(exerciseViewModels);
            Index = index;
        }
    }
}
