using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Models;
using GymTracker.Repositories;
using GymTracker.Services;
using Prism.Commands;
using Prism.Navigation;

namespace GymTracker.ViewModels
{
    public class AddTrainingViewModel : ViewModelBase
    {
        private readonly ITrainingRepository _trainingRepository;
        public DelegateCommand AddTrainingCommand { get;}

        public AddTrainingViewModel(INavigationService navigationService, ITrainingRepository trainingRepository) 
            : base(navigationService)
        {
            _trainingRepository = trainingRepository;
            AddTrainingCommand = new DelegateCommand(async () => await AddTraining());
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private async Task AddTraining()
        {
            try
            {
                var training = new Training(Name);
                await _trainingRepository.SaveItemAsync(training);
                var backParams = new NavigationParameters {
                {
                    "newTraining", training
                }};
                await NavigationService.GoBackAsync(backParams);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
