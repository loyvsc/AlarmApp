using AlarmApp.PageModels;
using AlarmApp.Services;
using FreshMvvm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AlarmApp
{
    public partial class App : Application
    {
        private readonly AlarmStorageService _alarmStorage = new AlarmStorageService();

        public App()
        {
            InitializeComponent();

            AlarmStorageService.InitSettings();
            GetAllAlarmTones();

            SetUpIoC();
            /*
			 * TabbedPage page 
			 */
            FreshTabbedFONavigationContainer tabbedNavigation = new FreshTabbedFONavigationContainer("Alarm App")
            {
                BackgroundColor = (Color)Resources["PrimaryColor"]
            };

            tabbedNavigation.AddTab<AlarmListPageModel>("Today's Alarms", null, AlarmListType.Today);
            tabbedNavigation.AddTab<AlarmListPageModel>("All Alarms", null, AlarmListType.All);
            MainPage = tabbedNavigation;
            /*
			 * Single page
			 */

            //var page = FreshPageModelResolver.ResolvePageModel<AlarmListPageModel>();
            //var nav = new FreshNavigationContainer(page);
            //nav.BackgroundColor = (Color)Resources["PrimaryColor"];
            //page.Title = "My Alarms";

            //MainPage = nav;


            //testing
            //MainPage = new TestPage();
        }

        private void GetAllAlarmTones()
        {
            var alarmList = _alarmStorage.GetAllTones();

            if (alarmList == null || alarmList.Count < Defaults.Tones.Count)
            {
                _alarmStorage.SetDefaultTones();
            }
        }

        private void SetUpIoC()
        {
            FreshIOC.Container.Register<IAlarmStorageService, AlarmStorageService>();
        }
    }
}