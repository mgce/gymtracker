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

        public SetsViewModel(Set set)
        {
            _set = set;
        }
    }
}
