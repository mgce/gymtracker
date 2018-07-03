using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Helpers;
using GymTracker.Models;
using GymTracker.Repositories;
using GymTracker.Services;
using Prism.Commands;
using Prism.Navigation;

namespace GymTracker.ViewModels
{
    public class AddStageViewModel : ViewModelBase
    {
        private readonly IStageRepository _stageRepository;
        public DelegateCommand AddStageCommand { get;}

        public AddStageViewModel(INavigationService navigationService, IStageRepository stageRepository) 
            : base(navigationService)
        {
            _stageRepository = stageRepository;
            AddStageCommand = new DelegateCommand(async () => await AddStage());
        }

        private int _trainingId;
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private async Task AddStage()
        {
            try
            {
                var stage = new Stage(Name, _trainingId);
                await _stageRepository.SaveItemAsync(stage);
                var backParams = new NavigationParameters {
                {
                    Constants.Models.NewStage, stage
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
