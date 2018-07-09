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
        private readonly IExerciseRepository _exerciseRepository;
        public DelegateCommand AddNewExerciseCommand { get; }

        public ExercisesViewModel(INavigationService navigationService, IExerciseRepository exerciseRepository) 
            : base(navigationService)
        {
            _exerciseRepository = exerciseRepository;
            Exercises = new ObservableCollection<Exercise>();
            AddNewExerciseCommand = new DelegateCommand(async ()=>await  AddNewExercise());
        }

        private ObservableCollection<Exercise> _exercises;
        public ObservableCollection<Exercise> Exercises
        {
            get => _exercises;
            set => SetProperty(ref _exercises, value);
        }

        private Stage _stages;
        public Stage Stage
        {
            get => _stages;
            set => SetProperty(ref _stages, value);
        }

        private async Task LoadExercises()
        {
            var exercises = await _exerciseRepository.GetByStageId(Stage.Id);
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
                Stage = parameters.GetValue<Stage>(Constants.Models.Stage);
                Title = $"{Stage.Name} exercises";
            }

            if (parameters.ContainsKey(Constants.Models.NewExercise))
            {
                var exercise = parameters.GetValue<Exercise>(Constants.Models.NewExercise);
                Exercises.Add(exercise);
            }

            await LoadExercises();
        }
    }
}
