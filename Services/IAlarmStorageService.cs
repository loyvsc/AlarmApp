﻿using AlarmApp.Models;
using Realms;
using System.Collections.Generic;

namespace AlarmApp.Services
{
    public interface IAlarmStorageService
    {
        Realm Realm { get; }
        Alarm GetAlarm(string id);
        List<Alarm> GetAllAlarms();
        List<Alarm> GetTodaysAlarms();
        void AddAlarm(Alarm alarm);
        void UpdateAlarm(Alarm alarm);
        void DeleteAlarm(Alarm alarm);
        bool DoesAlarmExist(Alarm alarm);
        void DeleteAllAlarms();
        Settings GetSettings();
        void SetDefaultTones();
        List<AlarmTone> GetAllTones();
        void AddTone(AlarmTone alarmTone);
        void DeleteTone(AlarmTone alarmTone);
        AlarmTone GetTone(string id);
    }
}