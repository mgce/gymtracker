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
        private readonly IExerciseTemplateRepository _exerciseTemplateRepository;
        public DelegateCommand AddExerciseCommand { get; }

        public AddExerciseViewModel(INavigationService navigationService, IExerciseTemplateRepository exerciseTemplateRepository) 
            : base(navigationService)
        {
            _exerciseTemplateRepository = exerciseTemplateRepository;
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
        public string Sets
        {
            get => _sets.ToString();
            set
            {
                try
                {
                    _sets = int.Parse(value);
                    var setsAsString = _sets.ToString();
                    SetProperty(ref setsAsString, value);
                }
                catch
                {
                    string setsAsString = null;
                    SetProperty(ref setsAsString, value);
                }

            }
        }

        private bool _useTimer;

        public bool UseTimer
        {
            get => _useTimer;
            set => SetProperty(ref _useTimer, value);
        }

        private int? _repetitions;
        public string Repetitions
        {
            get => _repetitions.ToString();
            set
            {
                try
                {
                    _repetitions = int.Parse(value);
                    var repetitionsAsString = _repetitions.ToString();
                    SetProperty(ref repetitionsAsString, value);
                }
                catch
                {
                    string repetitionsAsString = null;
                    SetProperty(ref repetitionsAsString, value);
                }

            }
        }

        private int? _minTime;
        public string MinTime
        {
            get => _minTime.ToString();
            set
            {
                try
                {
                    _minTime = int.Parse(value);
                    var minTimeAsString = _minTime.ToString();
                    SetProperty(ref minTimeAsString, value);
                }
                catch
                {
                    string minTimeAsString = null;
                    SetProperty(ref minTimeAsString, value);
                }

            }
        }

        private int? _maxTime;
        public string MaxTime
        {
            get => _maxTime.ToString();
            set
            {
                try
                {
                    _maxTime = int.Parse(value);
                    var maxTimeAsString = _maxTime.ToString();
                    SetProperty(ref maxTimeAsString, value);
                }
                catch
                {
                    string maxTimeAsString = null;
                    SetProperty(ref maxTimeAsString, value);
                }
                
            }
        }

        private async Task AddExercise()
        {
            var exercise = new ExerciseTemplate(Name, _repetitions, _sets.Value, UseTimer, _minTime, _maxTime, _stageId);
            await _exerciseTemplateRepository.SaveItemAsync(exercise);
            var navigationParams = new NavigationParameters
            {
                {Constants.Models.NewExercise, exercise}
            };
            await NavigationService.GoBackAsync(navigationParams);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey(Constants.Models.Stage))
            {
                _stageId = parameters.GetValue<StageTemplate>(Constants.Models.Stage).Id;
            }
            //TODO Walidacja jezeli nie ma stagea
        }
    }
}
