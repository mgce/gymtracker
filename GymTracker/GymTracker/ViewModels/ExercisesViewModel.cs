using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Helpers;
using GymTracker.Models;
using GymTracker.Repositories;
using GymTracker.Views;
using Prism.Commands;
using Prism.Navigation;

namespace GymTracker.ViewModels
{
    public class ExercisesViewModel : ViewModelBase
    {
        private readonly IExerciseTemplateRepository _exerciseTemplateRepository;
        public DelegateCommand AddNewExerciseCommand { get; }

        public ExercisesViewModel(INavigationService navigationService, IExerciseTemplateRepository exerciseTemplateRepository) 
            : base(navigationService)
        {
            _exerciseTemplateRepository = exerciseTemplateRepository;
            Exercises = new ObservableCollection<ExerciseTemplate>();
            AddNewExerciseCommand = new DelegateCommand(async ()=>await  AddNewExercise());
        }

        private ObservableCollection<ExerciseTemplate> _exercises;
        public ObservableCollection<ExerciseTemplate> Exercises
        {
            get => _exercises;
            set => SetProperty(ref _exercises, value);
        }

        private StageTemplate _stages;
        public StageTemplate Stage
        {
            get => _stages;
            set => SetProperty(ref _stages, value);
        }

        private async Task LoadExercises()
        {
            var exercises = await _exerciseTemplateRepository.GetByStageTemplateId(Stage.Id);
            Exercises.AddRange(exercises);
        }

        private async Task AddNewExercise()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add(Constants.Models.Stage, Stage);
            await NavigationService.NavigateAsync(nameof(AddExercisePage), navigationParams);
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey(Constants.Models.Stage))
            {
                Stage = parameters.GetValue<StageTemplate>(Constants.Models.Stage);
                Title = $"{Stage.Name} exercises";
            }

            if (parameters.ContainsKey(Constants.Models.NewExercise))
            {
                var exercise = parameters.GetValue<ExerciseTemplate>(Constants.Models.NewExercise);
                Exercises.Add(exercise);
            }
        }
    }
}
