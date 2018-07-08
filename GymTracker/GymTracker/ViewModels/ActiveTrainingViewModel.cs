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
        private readonly IStageRepository _stageRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IPageDialogService _dialogService;
        public DelegateCommand GoToNextStageCommand { get; set; }
        public DelegateCommand GoToPreviousStageCommand { get; set; }
        private List<Stage> _stages;

        public ActiveTrainingViewModel(INavigationService navigationService, IStageRepository stageRepository, IExerciseRepository exerciseRepository, IPageDialogService dialogService) 
            : base(navigationService)
        {
            _stageRepository = stageRepository;
            _exerciseRepository = exerciseRepository;
            _dialogService = dialogService;
            GoToNextStageCommand = new DelegateCommand(async()=>await GoToNextStage());
            GoToPreviousStageCommand = new DelegateCommand(async()=>await GoToPreviousStage());
            Exercises = new ObservableCollection<ExerciseViewModel>();
        }

        private ObservableCollection<ExerciseViewModel> _exercises;
        public ObservableCollection<ExerciseViewModel> Exercises
        {
            get => _exercises;
            set => SetProperty(ref _exercises, value);
        }

        private Stage _currentStage;
        public Stage CurrentStage
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
                Training = parameters.GetValue<Training>(Constants.Models.Training);
                await Task.Run(() => _stageRepository.GetStagesByTrainingId(Training.Id)).ContinueWith(async stages =>
                       {
                           _stages = stages.Result;

                           if (_stages?.Count == 0)
                           {
                               await _dialogService.DisplayAlertAsync("Empty stages",
                                   "Sorry, but you not created any stage yet.", "Go Back");
                               return;
                               //await NavigationService.GoBackAsync();
                           }

                           CurrentStage = _stages.FirstOrDefault();
                           Index = _stages.IndexOf(CurrentStage);
                           var exercises = await _exerciseRepository.GetByStageId(CurrentStage.Id);
                           exercises.ForEach(x=>Exercises.Add(new ExerciseViewModel(x)));

                       });
            }
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
            CurrentStage = _stages[index];
            Exercises = new ObservableCollection<ExerciseViewModel>();
            var exervises = await _exerciseRepository.GetByStageId(CurrentStage.Id);
            exervises.ForEach(x=>Exercises.Add(new ExerciseViewModel(x)));
            Index = index;
        }
    }
}
