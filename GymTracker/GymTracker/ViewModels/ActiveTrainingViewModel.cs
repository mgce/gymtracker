using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Helpers;
using GymTracker.Models;
using GymTracker.Repositories;
using GymTracker.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace GymTracker.ViewModels
{
    public class ActiveTrainingViewModel : ViewModelBase
    {
        private readonly ITrainingRepository _trainingRepository;
        private readonly IActiveTrainingService _activeTrainingService;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IPageDialogService _dialogService;
        public DelegateCommand GoToNextStageCommand { get; set; }
        public DelegateCommand GoToPreviousStageCommand { get; set; }
        private List<StageTemplate> _stageTemplates;

        public ActiveTrainingViewModel(INavigationService navigationService, 
            ITrainingRepository trainingRepository,
            IActiveTrainingService activeTrainingService,
            IExerciseRepository exerciseRepository,
            IPageDialogService dialogService) 
            : base(navigationService)
        {
            _trainingRepository = trainingRepository;
            _activeTrainingService = activeTrainingService;
            _exerciseRepository = exerciseRepository;
            _dialogService = dialogService;
            GoToNextStageCommand = new DelegateCommand(async()=>await GoToNextStage());
            GoToPreviousStageCommand = new DelegateCommand(async()=>await GoToPreviousStage());
            GrouppedSets = new ObservableCollection<GrouppedSets>();
        }

        private ObservableCollection<GrouppedSets> _grouppedSets;
        public ObservableCollection<GrouppedSets> GrouppedSets
        {
            get => _grouppedSets;
            set => SetProperty(ref _grouppedSets, value);
        }

        private StageTemplate _currentStageTemplate;
        public StageTemplate CurrentStageTemplate
        {
            get => _currentStageTemplate;
            set => SetProperty(ref _currentStageTemplate, value);
        }

        private int _indexOfCurrentStage;
        public int IndexOfCurrentStage
        {
            get => _indexOfCurrentStage;
            set => SetProperty(ref _indexOfCurrentStage, value);
        }

        private Training _training;
        public Training Training
        {
            get => _training;
            set => SetProperty(ref _training, value);
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey(Constants.Models.Training))
            {
                var isNew = true;
                var trainingTemplate = parameters.GetValue<TrainingTemplate>(Constants.Models.Training);

                var activeTraining = await _trainingRepository.GetActiveTrainingByTemplateId(trainingTemplate.Id);
                if (activeTraining == null)
                {
                    await CreateTrainingFromTemplate(trainingTemplate);
                }
                else
                {
                    Training = activeTraining;
                    isNew = false;
                }
                //TODO ADD TRY-CATCH TO HANDLE EXCEPTION IN THIS METHOD BELOW
                _stageTemplates = await _activeTrainingService.LoadStageTemplates(trainingTemplate.Id);
                SetInitialCurrentStage();
                var firstStageTemplateId = _stageTemplates.First().Id;
                Device.BeginInvokeOnMainThread(async () => 
                    GrouppedSets = await _activeTrainingService.GetGrouppedSetFromStage(firstStageTemplateId, isNew));
            }
        }

        private void SetInitialCurrentStage()
        {
            CurrentStageTemplate = _stageTemplates.FirstOrDefault();
            IndexOfCurrentStage = _stageTemplates.IndexOf(CurrentStageTemplate);
        }

        private async Task CreateTrainingFromTemplate(TrainingTemplate template)
        {
            Training = new Training(template);
            Training.StartTraining();
            await _trainingRepository.SaveItemAsync(Training);
        }

        private async Task GoToNextStage()
        {
            var lastIndex = _stageTemplates.Count - 1;
            var nextIndex = IndexOfCurrentStage + 1;

            if (nextIndex <= lastIndex)
                await SwapStages(nextIndex);
        }

        private async Task GoToPreviousStage()
        {
            var previousIndex = IndexOfCurrentStage - 1;

            if (previousIndex >= 0)
                await SwapStages(previousIndex);
        }

        private async Task SwapStages(int index)
        {
            CurrentStageTemplate = _stageTemplates[index];
            GrouppedSets = new ObservableCollection<GrouppedSets>();

            var exercises = await _exerciseRepository.GetByStageTemplateId(CurrentStageTemplate.Id);
            var isNew = !exercises.Any();

            Device.BeginInvokeOnMainThread(async () => GrouppedSets = await _activeTrainingService.GetGrouppedSetFromStage(CurrentStageTemplate.Id, isNew));
            IndexOfCurrentStage = index;
        }
    }
}
