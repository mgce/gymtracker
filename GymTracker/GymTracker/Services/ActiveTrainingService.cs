using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Models;
using GymTracker.Repositories;
using GymTracker.ViewModels;

namespace GymTracker.Services
{
    public interface IActiveTrainingService
    {
        Task<List<StageTemplate>> LoadStageTemplates(int trainingTemplateId);
        Task<ObservableCollection<GrouppedSets>> GetGrouppedSetFromStage(int stageId, bool isNew);
    }

    public class ActiveTrainingService : IActiveTrainingService
    {
        private readonly ISetRepository _setRepository;
        private readonly IStageTemplateRepository _stageTemplateRepository;
        private readonly IExerciseTemplateRepository _exerciseTemplateRepository;
        private readonly ITrainingRepository _trainingRepository;
        private readonly IStageRepository _stageRepository;
        private readonly IExerciseRepository _exerciseRepository;

        public ActiveTrainingService(ISetRepository setRepository, 
            IStageTemplateRepository stageTemplateRepository, 
            IExerciseTemplateRepository exerciseTemplateRepository,
            ITrainingRepository trainingRepository,
            IStageRepository stageRepository,
            IExerciseRepository exerciseRepository)
        {
            _setRepository = setRepository;
            _stageTemplateRepository = stageTemplateRepository;
            _exerciseTemplateRepository = exerciseTemplateRepository;
            _trainingRepository = trainingRepository;
            _stageRepository = stageRepository;
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

        public async Task<ObservableCollection<GrouppedSets>> GetGrouppedSetFromStage(int stageTemplateId, bool isNew)
        {
            var exercisesList = isNew 
                ? await CreateExercisesFromStageTemplate(stageTemplateId) 
                : await GetExerciseByStageTemplateId(stageTemplateId);

            var exerciseViewModel = await CreateExerciseViewModelsFromExercises(exercisesList);

            return GetGrouppedSetsFromExerciseViewModel(exerciseViewModel, isNew);
        }

        private async Task<List<Exercise>> GetExerciseByStageTemplateId(int stageTemplateId)
        {
            var existingExercises = await _exerciseRepository.GetByStageTemplateId(stageTemplateId);
            var grouppedListOfExistingExercises = existingExercises.OrderByDescending(x => x.DateCreated).GroupBy(x => x.DateCreated.Date)
                .FirstOrDefault();

            var exercisesTemplateFromStage = await _exerciseTemplateRepository.GetByStageTemplateId(stageTemplateId);

            if (grouppedListOfExistingExercises == null)
                return await CreateExercisesFromStageTemplate(stageTemplateId);

            var numberOfExistingExercises = grouppedListOfExistingExercises.Count();
            var numberOfExerciseTemplates = exercisesTemplateFromStage.Count;
            var numberOfNotCreatedExercises = numberOfExistingExercises - numberOfExerciseTemplates;

            if (numberOfNotCreatedExercises != 0)
                return existingExercises;

            var exerciseTemplatesToGenerate = exercisesTemplateFromStage.Skip(numberOfExistingExercises)
                .Take(numberOfNotCreatedExercises).ToList();

            var newCreatedExercises = await CreateExercisesFromTemplate(exerciseTemplatesToGenerate, stageTemplateId);
            newCreatedExercises.AddRange(grouppedListOfExistingExercises.Select(exercise=> exercise));
            return newCreatedExercises;

        }

        private async Task<List<Exercise>> CreateExercisesFromTemplate(List<ExerciseTemplate> templates, int stageTemplateId)
        {
            var exercisesList = new List<Exercise>();

            foreach (var template in templates)
            {
                var exercise = new Exercise(template, stageTemplateId);
                await _exerciseRepository.SaveItemAsync(exercise);
                exercisesList.Add(exercise);
            }

            return exercisesList;
        }

        private async Task<List<ExerciseViewModel>> CreateExerciseViewModelsFromExercises(List<Exercise> exercises)
        {
            var exerciseViewModels = new List<ExerciseViewModel>();
            var exerciseTemplates = await _exerciseTemplateRepository.GetByExerciseTemplateIds(exercises.Select(x=>x.ExerciseTemplateId).ToArray());
            foreach (var exercise in exercises)
            {
                exercise.Template = exerciseTemplates.Single(x => x.Id == exercise.ExerciseTemplateId);
                exerciseViewModels.Add(new ExerciseViewModel(exercise));
            }

            return exerciseViewModels;
        }

        private ObservableCollection<GrouppedSets> GetGrouppedSetsFromExerciseViewModel(List<ExerciseViewModel> exerciseViewModels, bool isNew)
        {
            var grouppedSetList = new ObservableCollection<GrouppedSets>();

            foreach (var exerciseViewModel in exerciseViewModels)
            {
                if(isNew)
                    CreateNewStages(exerciseViewModel, ref grouppedSetList);
                else
                    LoadExistingStages(exerciseViewModel, ref grouppedSetList);
            }

            return grouppedSetList;
        }

        private void CreateNewStages(ExerciseViewModel exerciseViewModel, ref ObservableCollection<GrouppedSets> grouppedSetList)
        {
            var grouppedSet = new GrouppedSets(exerciseViewModel);

            for (var i = 0; i < exerciseViewModel.Sets; i++)
            {
                var set = new Set(exerciseViewModel.ExerciseId, i, exerciseViewModel.Repetition, exerciseViewModel.MinTime, exerciseViewModel.MaxTime);
                _setRepository.SaveItemAsync(set);
                grouppedSet.Items.Add(new SetsViewModel(set));
            }
            grouppedSetList.Add(grouppedSet);
        }

        private void LoadExistingStages(ExerciseViewModel exerciseViewModel, ref ObservableCollection<GrouppedSets> grouppedSetList)
        {
            var grouppedSet = new GrouppedSets(exerciseViewModel);
            var sets = _setRepository.GetSetsByExerciseId(exerciseViewModel.ExerciseId).Result;
            sets = sets.OrderByDescending(x => x.Order).ToList();
            foreach (var set in sets)
            {
                grouppedSet.Items.Add(new SetsViewModel(set));
            }
            grouppedSetList.Add(grouppedSet);
        }

        private async Task GetPreviousSets(int stageId)
        {
            var stage = await _stageRepository.GetItemAsync(stageId);
            var stageTemplateId = stage.StageTemplateId;
            var stageTemplate = await _stageTemplateRepository.GetItemAsync(stageTemplateId);
            var lastTraining = _trainingRepository.GetTrainingByTemplateId(stageTemplate.TrainingTemplateId);
            var lastStages = _stageRepository.GetByTrainingId(lastTraining.Id);
        }

    }
}
