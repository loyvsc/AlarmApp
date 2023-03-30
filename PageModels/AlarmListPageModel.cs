using AlarmApp.Models;
using AlarmApp.Services;
using FreshMvvm;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AlarmApp.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class AlarmListPageModel : FreshBasePageModel
    {
        private AlarmListType _alarmListType;
        private IAlarmStorageService _alarmStorage;
        private Alarm _selectedAlarm;

        public ObservableCollection<Alarm> Alarms { get; set; } = new ObservableCollection<Alarm>();

        public Alarm SelectedAlarm
        {
            get => _selectedAlarm;
            set
            {
                _selectedAlarm = value;
                if (value != null)
                {
                    OpenPage(value);
                }
            }
        }

        public ICommand NewAlarmCommand
        {
            get => new FreshAwaitCommand(async (o, tcs) =>
            {
                await CoreMethods.PushPageModel<NewAlarmPageModel>(null, false, true);
                tcs.SetResult(true);
            });
        }        

        public ICommand DeleteAlarmCommand
        {
            get => new Xamarin.Forms.Command((param) =>
            {
                Alarm alarm = (Alarm)param;
                Alarms.Remove(alarm);
                _alarmStorage.DeleteAlarm(alarm);
            });
        }

        public ICommand SettingsCommand
        {
            get => new FreshAwaitCommand(async (o, tcs) =>
            {
                await CoreMethods.PushPageModel<SettingsPageModel>(null, false, true);
                tcs.SetResult(true);
            });
        }

        public AlarmListPageModel(IAlarmStorageService alarmStorage)
        {
            _alarmStorage = alarmStorage;
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            if (initData != null)
            {
                _alarmListType = (AlarmListType)initData;
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            CreateLists();
        }

        private async void OpenPage(Alarm selectedAlarm)
        {
            await CoreMethods.PushPageModel<ViewAlarmPageModel>(selectedAlarm, false, true);
        }

        public override void ReverseInit(object returnedData)
        {
            base.ReverseInit(returnedData);

            if ((bool)returnedData)
            {
                Alarms.Clear();
                CreateLists();
            }
        }

        private void CreateLists()
        {
            if (_alarmListType == AlarmListType.Today)
            {
                Alarms = new ObservableCollection<Alarm>(_alarmStorage.GetTodaysAlarms());
            }
            else
            {
                Alarms = new ObservableCollection<Alarm>(_alarmStorage.GetAllAlarms());
            }
        }
    }
}