﻿using PropertyChanged;
using Realms;
using System;
using System.Linq;

namespace AlarmApp.Models
{
    [AddINotifyPropertyChangedInterface]
    public class DaysOfWeek : RealmObject
    {
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        public bool[] AllDays => new bool[] { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday };

        public DaysOfWeek() { }

        public DaysOfWeek(bool[] allDays)
        {
            if (allDays.Length != 7)
            {
                return;
            }

            Monday = allDays[0];
            Tuesday = allDays[1];
            Wednesday = allDays[2];
            Thursday = allDays[3];
            Friday = allDays[4];
            Saturday = allDays[5];
            Sunday = allDays[6];
        }

        public DaysOfWeek(bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday) : this(new bool[] { monday, tuesday, wednesday, thursday, friday, saturday, sunday }) { }

        public static bool GetHasADayBeenSelected(DaysOfWeek days)
        {
            if (days == null)
            {
                return false;
            }
            return days.AllDays.Contains(true);
        }

        public override bool Equals(object obj)
        {
            if (obj is DayOfWeek)
            {
                int dayOfWeek = (int)obj;
                if (dayOfWeek == 0)
                {
                    return Sunday;
                }
                else
                {
                    bool day = AllDays[dayOfWeek - 1];
                    return day;
                }
            }

            if (obj is DaysOfWeek)
            {
                DaysOfWeek daysOfWeek = obj as DaysOfWeek;
                if (this.AllDays == daysOfWeek.AllDays)
                {
                    return true;
                }
            }
            return false;
        }
    }
}