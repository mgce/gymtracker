using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Helpers;
using GymTracker.Models;
using GymTracker.Repositories;
using Prism.Commands;
using Prism.Navigation;

namespace GymTracker.ViewModels
{
    public class AddExerciseViewModel : ViewModelBase
    {
        private readonly IExerciseRepository _exerciseRepository;
        public DelegateCommand AddExerciseCommand { get; }

        public AddExerciseViewModel(INavigationService navigationService, IExerciseRepository exerciseRepository) 
            : base(navigationService)
        {
            _exerciseRepository = exerciseRepository;
            Title = "Add Exercise";
            AddExerciseCommand = new DelegateCommand(async () => await AddExercise());
        }

        private int _stageId;

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private int? _sets;

        public int? Sets
        {
            get => _sets;
            set => SetProperty(ref _sets, value);
        }

        private bool _useTimer;

        public bool UseTimer
        {
            get => _useTimer;
            set => SetProperty(ref _useTimer, value);
        }

        private int? _repetitions;

        public int? Repetitions
        {
            get => _repetitions;
            set => SetProperty(ref _repetitions, value);
        }

        private int? _minTime;

        public int? MinTime {
            get => _minTime;
            set => SetProperty(ref _minTime, value);
        }

        private int? _maxTime;

        public int? MaxTime
        {
            get => _maxTime;
            set => SetProperty(ref _maxTime, value);
        }

        private async Task AddExercise()
        {
            var exercise = new Exercise(Name, Repetitions, UseTimer, MinTime, MaxTime, _stageId);
            await _exerciseRepository.SaveItemAsync(exercise);
            var navigationParams = new NavigationParameters
            {
                {Constants.Models.NewExercise, exercise}
            };
            await NavigationService.GoBackAsync(navigationParams);
        }

    }
}
