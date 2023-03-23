using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;

namespace AlarmApp.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Settings : RealmObject
    {
        public int FormatInt { get; private set; }

        [Ignored]
        public ClockFormat Format
        {
            get => (ClockFormat)FormatInt;
            set => FormatInt = (int)value;
        }

        [Ignored]
        public string TimeFormat => GetTimeFormatAsString();
        public AlarmTone AlarmTone { get; set; }
        public IList<AlarmTone> AllAlarmTones { get; }
        public bool IsVibrateOn { get; set; } = true;

        /// <summary>
        /// Gets the Format in string format
        /// </summary>
        /// <returns>The time format as string</returns>
        private string GetTimeFormatAsString()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            switch (Format)
            {
                case ClockFormat.Hour24:
                    {
                        stringBuilder.Append("24 Hour - ");
                        stringBuilder.Append(DateTime.Now.TimeOfDay.ToString(@"hh\:mm"));
                        break;
                    }
                case ClockFormat.Hour12:
                    {
                        stringBuilder.Append("12 Hour - ");
                        stringBuilder.Append(DateTime.Now.ToString("h:mm tt"));
                        break;
                    }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Switches the format between 24 Hour and 12 Hour
        /// </summary>
        public void SwitchFormat()
        {
            if (Format == ClockFormat.Hour12)
            {
                Format = ClockFormat.Hour24;
            }
            else
            {
                Format = ClockFormat.Hour12;
            }
        }
    }

    public enum ClockFormat
    {
        Hour12 = 0,
        Hour24 = 1
    }
}