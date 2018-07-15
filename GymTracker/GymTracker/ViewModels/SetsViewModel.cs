using System;
using System.Collections.Generic;
using System.Text;
using GymTracker.Models;
using GymTracker.Repositories;
using Prism.Commands;
using Prism.Mvvm;

namespace GymTracker.ViewModels
{
    public class SetsViewModel : BindableBase
    {
        private readonly ISetRepository _setRepository;
        public DelegateCommand SetDoneCommand;

        private Set _set;

        public SetsViewModel(ISetRepository setRepository)
        {
            _setRepository = setRepository;
            SetDoneCommand = new DelegateCommand(SetDone);
        }

        public int Order { get => _set.Order;}

        private string _name;
        public string Name
        {
            get => _name;
        }

        private string _previous;
        public string Previous
        {
            get => _previous;
        }

        private int? _repetitions;
        public int? Repetitions {
            get => _repetitions;
            set => SetProperty(ref _repetitions, value);
        }

        private bool _done;
        public bool Done
        {
            get => _done;
            set {
                SetProperty(ref _done, value);
                _set.Done = value;
            }
        }

        public SetsViewModel(Set set)
        {
            _set = set;
            _repetitions = _set.Repetitions;
        }

        public void SetDone()
        {
            Done = !Done;
            _setRepository.SaveItemAsync(_set);
        }
    }
}
