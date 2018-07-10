using System.Collections.ObjectModel;
using GymTracker.ViewModels;

namespace GymTracker.Models
{
    public class GrouppedSets : ObservableCollection<SetsViewModel>
    {
        public ExerciseViewModel ExerciseViewModel { get; set; }

        public GrouppedSets(ExerciseViewModel exerciseViewModel)
        {
            ExerciseViewModel = exerciseViewModel;
        }
    }
}
