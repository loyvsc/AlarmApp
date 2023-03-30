using AlarmApp.Models;
using AlarmApp.Services;
using FreshMvvm;
using Realms;
using System;
using System.Windows.Input;

namespace AlarmApp.PageModels
{
    public class ViewAlarmPageModel : AlarmBasePageModel
    {
        private readonly AlarmStorageService alarmStorage = new AlarmStorageService();
        public ICommand UpdateAlarmCommand
        {
            get => new FreshAwaitCommand((tcs) =>
            {
                UpdateAlarm();
                tcs.SetResult(true);
            });
        }

        public ViewAlarmPageModel(IAlarmStorageService alarmStorage) : base(alarmStorage) { }

        public override void Init(object initData)
        {
            base.Init(initData);
            Alarm = (Alarm)initData;
            AlarmTone = AlarmStorage.GetTone(Alarm.Tone);

            if (AlarmTone == null)
            {
                AlarmTone = AlarmStorage.GetAllTones()[0];
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            //set the properties of this PageModel to the alarm passed through
            var freq = Alarm.GetNumberAndPeriodFromTimeSpan(Alarm.Frequency);
            FrequencyNumber = freq.Key;
            FrequencyPeriod = freq.Value;
            var duration = Alarm.GetNumberAndPeriodFromTimeSpan(Alarm.Duration);
            DurationNumber = duration.Key;
            DurationPeriod = duration.Value;
            IsVibrateOn = Alarm.IsVibrateOn;
            Name = Alarm.Name;
            Time = Alarm.Time;
            Days = new DaysOfWeek(Alarm.Days.AllDays);
        }

        /// <summary>
        /// Updates the alarm with the values edited by the user
        /// </summary>
        private void UpdateAlarm()
        {
            //need UI feedback
            if (!ValidateFields())
            {
                return;
            }

            TimeSpan frequency = Alarm.GetFrequencyDurationFromNumberAndPeriod(FrequencyNumber, FrequencyPeriod);
            TimeSpan duration = Alarm.GetFrequencyDurationFromNumberAndPeriod(DurationNumber, DurationPeriod);
            Realm realm = Realms.Realm.GetInstance();

            realm.Write(() =>
            {
                Alarm.Name = Name;
                Alarm.Frequency = frequency;
                Alarm.Time = Time;
                Alarm.Days = Days;
                Alarm.Duration = duration;
                Alarm.Tone = AlarmTone.Id;
                Alarm.IsVibrateOn = IsVibrateOn;
            });

            CoreMethods.PopPageModel(true, false, true);
        }

        protected override bool ValidateFields()
        {
            bool isFieldsValidate = base.ValidateFields();
            bool isAnyDaySelected = true;

            if (!DaysOfWeek.GetHasADayBeenSelected(Days))
            {
                HasDayBeenSelected = false;
                isAnyDaySelected = false;
            }

            return isFieldsValidate & isAnyDaySelected;
        }
    }
}