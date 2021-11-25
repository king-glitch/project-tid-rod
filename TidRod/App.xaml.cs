using TidRod.Models;
using TidRod.Services.DataStore.Firebase;
using TidRod.Services.DataStore.Mock;
using TidRod.Services.Interface;
using Xamarin.Forms;

// registering fonts
[assembly: ExportFont("Material.MaterialIconsRegular.ttf", Alias = "Material")]
[assembly: ExportFont("Roboto.RobotoBlack.ttf", Alias = "Roboto")]
[assembly: ExportFont("Roboto.RobotoBlackItalic.ttf", Alias = "Roboto")]
[assembly: ExportFont("Roboto.RobotoBold.ttf", Alias = "Roboto")]
[assembly: ExportFont("Roboto.RobotoBoldItalic.ttf", Alias = "Roboto")]
[assembly: ExportFont("Roboto.RobotoItalic.ttf", Alias = "Roboto")]
[assembly: ExportFont("Roboto.RobotoLight.ttf", Alias = "Roboto")]
[assembly: ExportFont("Roboto.RobotoLightItalic.ttf", Alias = "Roboto")]
[assembly: ExportFont("Roboto.RobotoMedium.ttf", Alias = "Roboto")]
[assembly: ExportFont("Roboto.RobotoMediumItalic.ttf", Alias = "Roboto")]
[assembly: ExportFont("Roboto.RobotoRegular.ttf", Alias = "Roboto")]
[assembly: ExportFont("Roboto.RobotoThin.ttf", Alias = "Roboto")]
[assembly: ExportFont("Roboto.RobotoThinItalic.ttf", Alias = "Roboto")]

namespace TidRod
{
    public partial class App : Application
    {
        public static bool IsAuthenticated = false;

        public static string CurrentSession = null;

        public App()
        {
            InitializeComponent();
            //DependencyService.Register<MockUserDataStore>();
            //DependencyService.Register<MockCarDataStore>();

            DependencyService.Register<ICarDataStore<Car>, FirebaseCarDataStore>();
            DependencyService.Register<IUserDataStore<User>, FirebaseUserDataStore>();
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
