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
        private readonly IStageTemplateRepository _stageTemplateRepository;
        private readonly IExerciseTemplateRepository _exerciseTemplateRepository;
        private readonly ITrainingRepository _trainingRepository;
        private readonly IStageRepository _stageRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ISetRepository _setRepository;
        private readonly IActiveTrainingService _activeTrainingService;
        private readonly IPageDialogService _dialogService;
        public DelegateCommand GoToNextStageCommand { get; set; }
        public DelegateCommand GoToPreviousStageCommand { get; set; }
        private List<StageTemplate> _stageTemplates;

        public ActiveTrainingViewModel(INavigationService navigationService, 
            IStageTemplateRepository stageTemplateRepository, 
            IExerciseTemplateRepository exerciseTemplateRepository, 
            ITrainingRepository trainingRepository,
            IStageRepository stageRepository,
            IExerciseRepository exerciseRepository,
            ISetRepository setRepository,
            IActiveTrainingService activeTrainingService,
            IPageDialogService dialogService) 
            : base(navigationService)
        {
            _stageTemplateRepository = stageTemplateRepository;
            _exerciseTemplateRepository = exerciseTemplateRepository;
            _trainingRepository = trainingRepository;
            _stageRepository = stageRepository;
            _exerciseRepository = exerciseRepository;
            _setRepository = setRepository;
            _activeTrainingService = activeTrainingService;
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

        private StageTemplate _currentStage;
        public StageTemplate CurrentStage
        {
            get => _currentStage;
            set => SetProperty(ref _currentStage, value);
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
                _stageTemplates = await _activeTrainingService.LoadStageTemplates(trainingTemplate.Id);
                SetInitialCurrentStage();
                var firstStageTemplateId = _stageTemplates.First().Id;
                Device.BeginInvokeOnMainThread(async () => 
                    GrouppedSets = await _activeTrainingService.GetGrouppedSetFromStage(firstStageTemplateId, isNew));
            }
        }

        private void SetInitialCurrentStage()
        {
            CurrentStage = _stageTemplates.FirstOrDefault();
            IndexOfCurrentStage = _stageTemplates.IndexOf(CurrentStage);
        }

        private async Task CreateTrainingFromTemplate(TrainingTemplate template)
        {
            Training = new Training(template);
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
            CurrentStage = _stageTemplates[index];
            GrouppedSets = new ObservableCollection<GrouppedSets>();

            Device.BeginInvokeOnMainThread(async () => GrouppedSets = await _activeTrainingService.GetGrouppedSetFromStage(CurrentStage.Id, false));
            IndexOfCurrentStage = index;
        }
    }
}
