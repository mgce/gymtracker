using System.Collections.ObjectModel;
using GymTracker.ViewModels;

namespace GymTracker.Models
{
    public class GrouppedSets : ObservableCollection<SetsViewModel>
    {
        public ExerciseViewModel ExerciseViewModel { get; set; }

        public ObservableCollection<SetsViewModel> Items => this;

        public GrouppedSets(ExerciseViewModel exerciseViewModel)
        {
            ExerciseViewModel = exerciseViewModel;
        }
    }
}
