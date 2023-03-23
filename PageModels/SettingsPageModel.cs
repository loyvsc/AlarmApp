using AlarmApp.Models;
using AlarmApp.Services;
using FreshMvvm;
using PropertyChanged;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AlarmApp.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class SettingsPageModel : FreshBasePageModel
    {
        IAlarmStorageService _alarmStorage;

        public Settings Settings { get; set; }

        public ICommand CellTappedCommand
        {
            get => new FreshAwaitCommand(async (param, tcs) =>
            {
                string parameter = (string)param;
                await OnCellTapped(parameter);

                tcs.SetResult(true);
            });
        }

        public SettingsPageModel(IAlarmStorageService alarmStorage)
        {
            _alarmStorage = alarmStorage;
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            Settings = _alarmStorage.GetSettings();
            GetAllAlarmTones();
        }

        private void GetAllAlarmTones()
        {
            var alarmList = _alarmStorage.GetAllTones();

            if (alarmList == null || alarmList.Count < Defaults.Tones.Count)
            {
                _alarmStorage.SetDefaultTones();
            }
        }


        private async Task OnCellTapped(string parameter)
        {
            switch (parameter)
            {
                case "Clock Format":
                    {
                        SwitchFormat();
                        break;
                    }
                case "Alarm Tone":
                    {
                        await GoToAlarmTonePage();
                        break;
                    }
                case "Vibrate":
                    {
                        ToggleVibrate();
                        break;
                    }
                case "Delete":
                    {
                        await DoDeleteAlert();
                        break;
                    }
            }
            return;
        }

        private async Task DoDeleteAlert()
        {
            bool shouldDeleteAlarms = await CoreMethods.DisplayAlert("Are you sure?",
                                           "You are about to delete all your alarms, " +
                                           "this action is permanent and cannot be undone.",
                                           "DELETE", "CANCEL");

            if (shouldDeleteAlarms)
            {
                DeleteAlarms();
            }
        }

        private void DeleteAlarms()
        {
            _alarmStorage.DeleteAllAlarms();
        }

        private async Task GoToAlarmTonePage()
        {
            await CoreMethods.PushPageModel<SettingsTonePageModel>(null, false, true);
        }

        private void ToggleVibrate()
        {
            _alarmStorage.Realm.Write(() => Settings.IsVibrateOn = !Settings.IsVibrateOn);
        }

        private void SwitchFormat()
        {
            _alarmStorage.Realm.Write(() => Settings.SwitchFormat());
        }
    }
}