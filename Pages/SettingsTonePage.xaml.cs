﻿using AlarmApp.Models;
using AlarmApp.PageModels;
using Xamarin.Forms;

namespace AlarmApp.Pages
{
    public partial class SettingsTonePage : ContentPage
    {
        public SettingsTonePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ToneListView.ItemSelected += ToneListView_ItemSelected;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            ToneListView.ItemSelected -= ToneListView_ItemSelected;
            base.OnDisappearing();
        }

        private void ToneListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //ToneListView.SelectedItem = null;
        }

        private void ViewCellBindingContextChanged(object sender, System.EventArgs e)
        {
            base.OnBindingContextChanged();
            ViewCell viewCell = (ViewCell)sender;

            if (!(viewCell.BindingContext is AlarmTone))
            {
                return;
            }

            AlarmTone alarmTone = viewCell.BindingContext as AlarmTone;
            viewCell.ContextActions.Clear();

            if (alarmTone == null)
            {
                return;
            }

            bool isDefaultTone = !alarmTone.IsCustomTone;

            if (isDefaultTone)
            {
                return;
            }

            viewCell.ContextActions.Add(new MenuItem
            {
                Text = "Delete",
                Icon = "delete",
                IsDestructive = true,
                Command = (BindingContext as SettingsTonePageModel).DeleteToneCommand,
                CommandParameter = alarmTone
            });
        }
    }
}