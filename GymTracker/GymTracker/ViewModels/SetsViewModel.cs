using System;
using System.Collections.Generic;
using System.Text;
using GymTracker.Models;
using Prism.Mvvm;

namespace GymTracker.ViewModels
{
    public class SetsViewModel : BindableBase
    {
        private Set _set;

        public int Order { get => _set.Order;}

        private string _previous;
        public string Previous
        {
            get => _previous;
        }

        private int _repetitions;
        public int Repetitions {
            get => _repetitions;
            set => SetProperty(ref _repetitions, value);
        }

        private bool _done;
        public bool Done
        {
            get => _done;
            set => SetProperty(ref _done, value);
        }

        public SetsViewModel(Set set)
        {
            _set = set;
            _repetitions = _set.Reps;
        }
    }
}
