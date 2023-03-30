using AlarmApp.Models;
using AlarmApp.Services;
using FreshMvvm;
using Realms;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlarmApp.PageModels
{
    public class NewAlarmPageModel : AlarmBasePageModel
    {
        private readonly IAlarmSetter _alarmSetter = DependencyService.Get<IAlarmSetter>();

        public ICommand SaveAlarmCommand
        {
            get => new FreshAwaitCommand((tcs) => { SaveAlarm(); tcs.SetResult(true); });
        }

        public NewAlarmPageModel(IAlarmStorageService alarmStorage) : base(alarmStorage)
        {
            Alarm = new Alarm();
            AlarmTone = alarmStorage.GetTone(Alarm.Tone);
            Alarm.Time = DateTime.Now.TimeOfDay;
        }

        /// <summary>
        /// Save a new alarm to the list
        /// </summary>
        private void SaveAlarm()
        {
            if (!ValidateFields())
            {
                return;
            }

            TimeSpan? frequency = GetDurationOrFrequency(FrequencyNumber, FrequencyPeriod);
            TimeSpan? duration = GetDurationOrFrequency(DurationNumber, DurationPeriod);

            Alarm.IsActive = true;
            Alarm.Frequency = (TimeSpan)frequency;
            Alarm.Duration = (TimeSpan)duration;

            _alarmSetter.SetAlarm(Alarm);

            Realms.Realm realm = Realms.Realm.GetInstance();

            using (Transaction transaction = realm.BeginWrite())
            {
                Alarm.Tone = AlarmTone.Id;
                realm.Add(Alarm, true);

                transaction.Commit();
            }

            CoreMethods.PopPageModel(true, false, true);
        }

        protected override bool ValidateFields()
        {
            bool isAnyDaySelected = true;
            if (!DaysOfWeek.GetHasADayBeenSelected(Alarm.Days))
            {
                HasDayBeenSelected = false;
                isAnyDaySelected = false;
            }

            bool isFieldsValidate = base.ValidateFields();
            return isFieldsValidate & isAnyDaySelected;
        }

        /// <summary>
        /// Get the duration or frequency as a TimeSpan object
        /// </summary>
        /// <returns>The frequency as a TimeSpan object, null if either are not set</returns>
        protected TimeSpan? GetDurationOrFrequency(int number, string period)
        {
            //need some sort of UI feedback for user
            if (number <= 0 || period == null || number == int.MaxValue)
            {
                return null;
            }

            TimeSpan time;

            if (period == "Minutes")
            {
                time = new TimeSpan(0, number, 0);
            }

            if (period == "Hours")
            {
                time = new TimeSpan(number, 0, 0);
            }

            return time;
        }
    }
}