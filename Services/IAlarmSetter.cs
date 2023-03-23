using AlarmApp.Models;
using System.Collections.Generic;

namespace AlarmApp.Services
{
    public interface IAlarmSetter
    {
        void SetAlarm(Alarm alarm);
        void SetRepeatingAlarm(Alarm alarm);
        void DeleteAlarm(Alarm alarm);
        void DeleteAllAlarms(List<Alarm> alarms);
    }
}