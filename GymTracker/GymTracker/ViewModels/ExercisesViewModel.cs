using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using GymTracker.Helpers;
using GymTracker.Models;
using Prism.Commands;
using Prism.Navigation;

namespace GymTracker.ViewModels
{
    public class ExercisesViewModel : ViewModelBase
    {
        public ExercisesViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
            Exercises = new ObservableCollection<Exercise>();
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

        public override void OnNavigatedTo(NavigationParameters parameters)
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
        }
    }
}
