using System;
using System.Collections.Generic;
using System.Text;
using GymTracker.Models;
using Prism.Mvvm;

namespace GymTracker.ViewModels
{
    public class ExerciseViewModel : BindableBase
    {
        private ExerciseTemplate _exerciseTemplate;
        public int ExerciseId { get; }

        public ExerciseViewModel(Exercise exercise)
        {
            _exerciseTemplate = exercise.Template;
            ExerciseId = exercise.Id;
        }

        private string _name;
        public string Name {
            get => _exerciseTemplate.Name;
            set => SetProperty(ref _name, value);
        }

        private int? _repetition;
        public int? Repetition
        {
            get => _exerciseTemplate.Repetition;
            set => SetProperty(ref _repetition, value);
        }

        private int _sets;
        public int Sets
        {
            get => _exerciseTemplate.Sets;
            set => SetProperty(ref _sets, value);
        }

        private bool _timer;
        public bool Timer
        {
            get => _exerciseTemplate.Timer;
            set => SetProperty(ref _timer, value);
        }

        private int? _minTime;
        public int? MinTime
        {
            get => _exerciseTemplate.MinTime;
            set => SetProperty(ref _minTime, value);
        }

        private int? _maxTime;
        public int? MaxTime
        {
            get => _exerciseTemplate.MaxTime;
            set => SetProperty(ref _maxTime, value);
        }
    }
}
