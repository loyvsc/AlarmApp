using AlarmApp.Models;
using AlarmApp.PageModels;
using AlarmApp.Services;
using FreshMvvm;
using System.Collections.Generic;
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

            Page page = FreshPageModelResolver.ResolvePageModel<AlarmListPageModel>();
            page.Title = "Будильник";
            FreshNavigationContainer mainPage = new FreshNavigationContainer(page)
            {
                BackgroundColor = (Color)Resources["PrimaryColor"]
            };

            MainPage = mainPage;
        }

        private void GetAllAlarmTones()
        {
            List<AlarmTone> alarmList = _alarmStorage.GetAllTones();

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