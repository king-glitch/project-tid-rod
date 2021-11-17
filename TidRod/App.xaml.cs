using TidRod.Models;
using TidRod.Services.DataStore.Firebase;
using TidRod.Services.DataStore.Mock;
using TidRod.Services.Interface;
using Xamarin.Forms;

// registering fonts
[assembly: ExportFont("Fonts/Roboto/Roboto-Black.ttf", Alias = "Roboto")]
[assembly: ExportFont("Fonts/Roboto/Roboto-BlackItalic.ttf", Alias = "Roboto")]
[assembly: ExportFont("Fonts/Roboto/Roboto-Bold.ttf", Alias = "Roboto")]
[assembly: ExportFont("Fonts/Roboto/Roboto-BoldItalic.ttf", Alias = "Roboto")]
[assembly: ExportFont("Fonts/Roboto/Roboto-Italic.ttf", Alias = "Roboto")]
[assembly: ExportFont("Fonts/Roboto/Roboto-Light.ttf", Alias = "Roboto")]
[assembly: ExportFont("Fonts/Roboto/Roboto-LightItalic.ttf", Alias = "Roboto")]
[assembly: ExportFont("Fonts/Roboto/Roboto-Medium.ttf", Alias = "Roboto")]
[assembly: ExportFont("Fonts/Roboto/Roboto-MediumItalic.ttf", Alias = "Roboto")]
[assembly: ExportFont("Fonts/Roboto/Roboto-Regular.ttf", Alias = "Roboto")]
[assembly: ExportFont("Fonts/Roboto/Roboto-Thin.ttf", Alias = "Roboto")]
[assembly: ExportFont("Fonts/Roboto/Roboto-ThinItalic.ttf", Alias = "Roboto")]

namespace TidRod
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockUserDataStore>();
            DependencyService.Register<MockCarDataStore>();

            // DependencyService.Register<ICarDataStore<Car>, FirebaseCarDataStore>();
            // DependencyService.Register<IUserDataStore<User>, FirebaseUserDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
