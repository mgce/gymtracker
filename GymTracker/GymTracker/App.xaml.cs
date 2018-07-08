using GymTracker.Models;
using GymTracker.Repositories;
using GymTracker.Services;
using Prism;
using Prism.Ioc;
using GymTracker.ViewModels;
using GymTracker.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Autofac;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GymTracker
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<AddTrainingPage, AddTrainingViewModel>();
            containerRegistry.RegisterForNavigation<ExercisesPage, ExercisesViewModel>();
            containerRegistry.RegisterForNavigation<StagesPage, StagesViewModel>();
            containerRegistry.RegisterForNavigation<AddExercisePage, AddExerciseViewModel>();
            containerRegistry.RegisterForNavigation<ActiveTrainingPage, ActiveTrainingViewModel>();

            containerRegistry.RegisterSingleton<ITrainingRepository, TrainingRepository>();
            containerRegistry.RegisterSingleton<IStageRepository, StageRepository>();
            containerRegistry.RegisterSingleton<IExerciseRepository, ExerciseRepository>();
        }
    }
}
