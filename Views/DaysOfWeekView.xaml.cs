﻿using AlarmApp.Models;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace AlarmApp.Views
{
    public partial class DaysOfWeekView : ContentView
    {
        private StringBuilder _sb;

        private string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

        public DaysOfWeekView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext == null)
            {
                return;
            }
            Alarm alarm = (Alarm)BindingContext;
            IsEnabled = alarm.IsActive;

            var daysOfWeek = alarm.Days;
            _sb = new StringBuilder();

            bool isEveryDay = alarm.Days.AllDays.All(X => X == true);

            if (isEveryDay)
            {
                DaysLabel.Text = "Every day";
                return;
            }

            for (int i = 0; i < daysOfWeek.AllDays.Length; i++)
            {
                bool hasDay = daysOfWeek.AllDays[i];
                if (hasDay)
                {
                    if (i > 0 && !string.IsNullOrWhiteSpace(_sb.ToString()))
                    {
                        _sb.Append(", ");
                    }
                    _sb.Append(days[i]);
                }
            }

            DaysLabel.Text = _sb.ToString();
            //MondayLabel.IsVisible = daysOfWeek.Monday;
            //TuesdayLabel.IsVisible = daysOfWeek.Tuesday;
            //WednesdayLabel.IsVisible = daysOfWeek.Wednesday;
            //ThursdayLabel.IsVisible = daysOfWeek.Thursday;
            //FridayLabel.IsVisible = daysOfWeek.Friday;
            //SaturdayLabel.IsVisible = daysOfWeek.Saturday;
            //SundayLabel.IsVisible = daysOfWeek.Sunday;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
            {
                SetLabelStyle(IsEnabled);
            }
        }

        private void SetLabelStyle(bool state)
        {
            if (state)
            {
                DaysLabel.Style = (Style)App.Current.Resources["AlarmExtrasHeading"];
            }
            else
            {
                DaysLabel.Style = (Style)App.Current.Resources["AlarmExtrasDisabledHeading"];
            }
        }
    }
}