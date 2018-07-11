using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Models;
using GymTracker.Repositories;
using GymTracker.ViewModels;

namespace GymTracker.Services
{
    public interface IActiveTrainingService
    {
        Task<List<Stage>> CreateStagesFromTemplate(List<StageTemplate> templates, int trainingId);
        Task<List<StageTemplate>> LoadStageTemplates(int trainingTemplateId);
        Task<ObservableCollection<GrouppedSets>> GetGrouppedSetFromStage(int stageId, bool isNew);
    }

    public class ActiveTrainingService : IActiveTrainingService
    {
        private readonly ISetRepository _setRepository;
        private readonly IStageRepository _stageRepository;
        private readonly IStageTemplateRepository _stageTemplateRepository;
        private readonly IExerciseTemplateRepository _exerciseTemplateRepository;
        private readonly IExerciseRepository _exerciseRepository;

        public ActiveTrainingService(ISetRepository setRepository, 
            IStageRepository stageRepository, 
            IStageTemplateRepository stageTemplateRepository, 
            IExerciseTemplateRepository exerciseTemplateRepository,
            IExerciseRepository exerciseRepository)
        {
            _setRepository = setRepository;
            _stageRepository = stageRepository;
            _stageTemplateRepository = stageTemplateRepository;
            _exerciseTemplateRepository = exerciseTemplateRepository;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<List<Exercise>> CreateExercisesFromStageTemplate(int stageTemplateId)
        {
            var exerciseTemplate = await
                _exerciseTemplateRepository.GetByStageTemplateId(stageTemplateId);
            return await CreateExercisesFromTemplate(exerciseTemplate, stageTemplateId);
        }

        public async Task<List<StageTemplate>> LoadStageTemplates(int trainingTemplateId)
        {
            var stageTemplate = await _stageTemplateRepository.GetStagesByTrainingTemplateId(trainingTemplateId);
            if(stageTemplate.Count == 0)
                throw new Exception("There is no stages in this training");

            return stageTemplate;
        }

        public async Task<List<Stage>> CreateStagesFromTemplate(List<StageTemplate> templates, int trainingId)
        {
            var stagesList = new List<Stage>();
            foreach (var template in templates)
            {
                var stage = new Stage(template, trainingId);
                await _stageRepository.SaveItemAsync(stage);
                stagesList.Add(stage);
            }

            return stagesList;
        }

        public async Task<ObservableCollection<GrouppedSets>> GetGrouppedSetFromStage(int stageId, bool isNew)
        {
            var exercisesList = isNew 
                ? await CreateExercisesFromStageTemplate(stageId) 
                : await _exerciseRepository.GetByStageId(stageId);

            var exerciseViewModel = CreateExerciseViewModelsFromExercises(exercisesList);

            return isNew
                ? CreateGrouppedSetsFromExerciseViewModels(exerciseViewModel)
                : LoadGrouppedSetsFromExerciseViewModel(exerciseViewModel);
        }

        private async Task<List<Exercise>> CreateExercisesFromTemplate(List<ExerciseTemplate> templates, int stageId)
        {
            var exercisesList = new List<Exercise>();

            foreach (var template in templates)
            {
                var exercise = new Exercise(template, stageId);
                await _exerciseRepository.SaveItemAsync(exercise);
                exercisesList.Add(exercise);
            }

            return exercisesList;
        }

        private List<ExerciseViewModel> CreateExerciseViewModelsFromExercises(List<Exercise> exercises)
        {
            var exerciseViewModels = new List<ExerciseViewModel>();
            foreach (var exercise in exercises)
            {
                exerciseViewModels.Add(new ExerciseViewModel(exercise));
            }

            return exerciseViewModels;
        }

        private ObservableCollection<GrouppedSets> LoadGrouppedSetsFromExerciseViewModel(List<ExerciseViewModel> exerciseViewModels)
        {
            var grouppedSetList = new ObservableCollection<GrouppedSets>();

            foreach (var exerciseViewModel in exerciseViewModels)
            {
                LoadExistingStages(exerciseViewModel, ref grouppedSetList);
            }

            return grouppedSetList;
        }

        private ObservableCollection<GrouppedSets> CreateGrouppedSetsFromExerciseViewModels(List<ExerciseViewModel> exerciseViewModels)
        {
            var grouppedSetList = new ObservableCollection<GrouppedSets>();

            foreach (var exerciseViewModel in exerciseViewModels)
            {
                CreateNewStages(exerciseViewModel, ref grouppedSetList);
            }

            return grouppedSetList;
        }

        private void CreateNewStages(ExerciseViewModel exerciseViewModel, ref ObservableCollection<GrouppedSets> grouppedSetList)
        {
            var grouppedSet = new GrouppedSets(exerciseViewModel);

            for (var i = 0; i < exerciseViewModel.Sets; i++)
            {
                var set = new Set(exerciseViewModel.ExerciseId, i);
                _setRepository.SaveItemAsync(set);
                grouppedSet.Items.Add(new SetsViewModel(set));
            }
            grouppedSetList.Add(grouppedSet);
        }

        private void LoadExistingStages(ExerciseViewModel exerciseViewModel, ref ObservableCollection<GrouppedSets> grouppedSetList)
        {
            var grouppedSet = new GrouppedSets(exerciseViewModel);
            var sets = _setRepository.GetSetsByExerciseId(exerciseViewModel.ExerciseId).Result;
            foreach (var set in sets)
            {
                grouppedSet.Items.Add(new SetsViewModel(set));
            }
            grouppedSetList.Add(grouppedSet);
        }

    }
}
