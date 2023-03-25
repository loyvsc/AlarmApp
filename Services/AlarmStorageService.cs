using AlarmApp.Models;
using Realms;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace AlarmApp.Services
{
    public class AlarmStorageService : IAlarmStorageService
    {
        private IAlarmSetter AlarmSetter { get; } = DependencyService.Get<IAlarmSetter>();

        public Realm Realm { get => Realm.GetInstance(); }

        public Alarm GetAlarm(string id)
        {
            return Realm.Find<Alarm>(id);
        }

        /// <returns>All alarms</returns>
        public List<Alarm> GetAllAlarms()
        {
            return Realm.All<Alarm>().ToList();
        }

        /// <returns>Today's alarms</returns>
        public List<Alarm> GetTodaysAlarms()
        {
            return Realm.All<Alarm>().ToList().Where(x => x.OccursToday == true).ToList();
        }

        /// <summary>
        /// Adds the alarm
        /// </summary>
        /// <param name="alarm">Alarm to add</param>
        public void AddAlarm(Alarm alarm)
        {            
            Realm.Write(() => Realm.Add(alarm));
        }

        /// <summary>
        /// Updates the alarm
        /// </summary>
        /// <param name="alarm">Alarm to update</param>
        public void UpdateAlarm(Alarm alarm)
        {
            Realm.Write(() => Realm.Add(alarm, true));
        }

        /// <param name="alarm">Alarm we want to delete</param>
        public void DeleteAlarm(Alarm alarm)
        {
            AlarmSetter.DeleteAlarm(alarm);
            Realm.Write(() => Realm.Remove(alarm));
        }

        /// <summary>
        /// Checks if the given alarm exists
        /// </summary>
        /// <returns><c>true</c>, if alarm was found, <c>false</c> otherwise</returns>
        /// <param name="alarm">The Alarm we want to know already exists</param>
        public bool DoesAlarmExist(Alarm alarm)
        {
            return Realm.All<Alarm>().Contains(alarm);
        }

        /// <summary>
        /// Deletes all the alarms
        /// </summary>
        public void DeleteAllAlarms()
        {
            AlarmSetter.DeleteAllAlarms(Realm.All<Alarm>().ToList());
            Realm.Write(() => Realm.RemoveAll<Alarm>());
        }

        /// <summary>
        /// Gets the settings
        /// </summary>
        /// <returns>The settings object</returns>
        public Settings GetSettings()
        {
            Settings readedSettings = Realm.All<Settings>().ToList()[0];

            if (readedSettings != null)
            {
                return readedSettings;
            }
            else
            {
                Settings settings = new Settings();
                Realm.Write(() => Realm.Add(readedSettings));
                return settings;
            }
        }

        public static void InitSettings()
        {
            var realm = Realm.GetInstance();
            var settingsList = realm.All<Settings>();

            if (settingsList.Count() <= 0)
            {
                realm.Write(() => realm.Add(new Settings()));
            }
        }

        public List<AlarmTone> GetAllTones()
        {
            return GetSettings().AllAlarmTones.ToList();
        }

        public void AddTone(AlarmTone alarmTone)
        {
            Realm.Write(() => GetSettings().AllAlarmTones.Add(alarmTone));
        }

        public void DeleteTone(AlarmTone alarmTone)
        {
            Realm.Write(() => GetSettings().AllAlarmTones.Remove(alarmTone));
        }

        public void SetDefaultTones()
        {
            Settings settings = GetSettings();

            Realm.Write(() =>
            {
                foreach (AlarmTone tone in Defaults.Tones)
                {
                    settings.AllAlarmTones.Add(tone);
                }
                settings.AlarmTone = Defaults.Tones[1];
            });
        }

        public AlarmTone GetTone(string id)
        {
            return Realm.Find<AlarmTone>(id);
        }
    }
}