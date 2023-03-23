using AlarmApp.Models;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;

namespace AlarmApp.Popups
{
    public partial class AlarmToneNamingPopupPage : PopupPage
    {
        private AlarmTone _toneToEdit;
        private bool _isEditMode;
        public event Action<string> ToneNameSet;

        public AlarmToneNamingPopupPage()
        {
            InitializeComponent();
        }

        public AlarmToneNamingPopupPage(AlarmTone tone)
        {
            InitializeComponent();
            _toneToEdit = tone;
            ToneNameEntry.Text = tone.Name;
            _isEditMode = true;
        }

        private void SetNameButtonPressed(object sender, System.EventArgs e)
        {
            if (_isEditMode)
            {
                Realms.Realm realm = Realms.Realm.GetInstance();
                realm.Write(() => _toneToEdit.Name = ToneNameEntry.Text);
            }
            else
            {
                ToneNameSet?.Invoke(ToneNameEntry.Text);
            }

            PopupNavigation.Instance.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            ToneNameSet?.Invoke("New Tone: " + DateTime.Now.ToString(@"dd MMM yy - hh\:mm"));
            PopupNavigation.Instance.PopAsync();
            return true;
        }

        protected override bool OnBackgroundClicked()
        {
            ToneNameSet?.Invoke("New Tone: " + DateTime.Now.ToString(@"dd MMM yy - hh\:mm"));
            return base.OnBackgroundClicked();
        }
    }
}