using System;
using System.Collections.Generic;
using System.Text;
using GymTracker.Models;
using Prism.Mvvm;

namespace GymTracker.ViewModels
{
    public class ExerciseViewModel : BindableBase
    {
        private Exercise _exercise;

        public ExerciseViewModel(Exercise exercise)
        {
            _exercise = exercise;
        }

        private string _name;
        public string Name {
            get => _exercise.Name;
            set => SetProperty(ref _name, value);
        }

        private int? _repetition;
        public int? Repetition
        {
            get => _exercise.Repetition;
            set => SetProperty(ref _repetition, value);
        }

        private bool _timer;
        public bool Timer
        {
            get => _exercise.Timer;
            set => SetProperty(ref _timer, value);
        }

        private int? _minTime;
        public int? MinTime
        {
            get => _exercise.MinTime;
            set => SetProperty(ref _minTime, value);
        }

        private int? _maxTime;
        public int? MaxTime
        {
            get => _exercise.MaxTime;
            set => SetProperty(ref _maxTime, value);
        }
    }
}
