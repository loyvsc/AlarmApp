using AlarmApp.Models;
using AlarmApp.Popups;
using AlarmApp.Services;
using FreshMvvm;
using PropertyChanged;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlarmApp.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class SettingsTonePageModel : FreshBasePageModel
    {
        private bool _isIndividualAlarmTone = false;
        private IFileLocator _fileLocator = Xamarin.Forms.DependencyService.Get<IFileLocator>();
        private readonly IPlaySoundService _soundService = DependencyService.Get<IPlaySoundService>();

        private Uri _newToneUri;
        private AlarmToneNamingPopupPage _namingPopupPage;

        private readonly IAlarmStorageService _alarmStorage;
        private AlarmTone _selectedTone;

        public Settings Settings { get; set; }

        public AlarmTone SelectedTone
        {
            get => _selectedTone;
            set
            {
                if (value != null)
                {
                    SetSelectedTone(value);
                }
            }
        }

        public bool FileNeedsNamed { get; set; }

        public ObservableCollection<AlarmTone> AllAlarmTones { get; set; }

        public ICommand ToneSavedCommand
        {
            get => new FreshAwaitCommand(async (obj, tcs) =>
            {
                AlarmTone selectedTone = _selectedTone;
                if (!_isIndividualAlarmTone)
                {
                    OnToneSelected(selectedTone);
                }
                await CoreMethods.PopPageModel(selectedTone, false, true);
                tcs.SetResult(true);
            });
        }

        public ICommand DeleteToneCommand
        {
            get => new Command<AlarmTone>((alarmTone) => DeleteAlarmTone(alarmTone));
        }

        public ICommand EditToneCommand
        {
            get => new Command<AlarmTone>((alarmTone) => EditAlarmTone(alarmTone));
        }

        public SettingsTonePageModel(IAlarmStorageService alarmStorage)
        {
            _alarmStorage = alarmStorage;
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            Settings = _alarmStorage.GetSettings();
            AllAlarmTones = new ObservableCollection<AlarmTone>(_alarmStorage.GetAllTones());

            // if we are setting an individual alarm's tone
            if (initData is Alarm newAlarm)
            {
                _isIndividualAlarmTone = true;
                _selectedTone = _alarmStorage.GetTone(newAlarm.Tone);
                RaisePropertyChanged("SelectedTone");
            }
        }

        private void PlayTone(AlarmTone tone)
        {
            _soundService.PlayAudio(tone);
        }

        /// <summary>
        /// Handles setting the current tone value
        /// </summary>
        /// <param name="value">Value.</param>
        private void SetSelectedTone(AlarmTone value)
        {
            bool isSelectedNull = value.Equals(default(AlarmTone)) || value == null;
            if (isSelectedNull)
            {
                _selectedTone = null;
                return;
            }

            //if the user selected the 'choose custom tone' option, display file explorer
            bool wasSelectCustomToneSelected = value.Equals(Defaults.Tones[0]);
            if (wasSelectCustomToneSelected)
            {
                try
                {
                    _fileLocator.FileChosen += ToneFileChosen;
                    _fileLocator.OpenFileLocator();
                }
                catch (Exception ex)
                {
                    App.Current.MainPage.DisplayAlert(ex.Message, ex.Message, "cancel");
                }

                return;
            }
            //_selectedTone = value;
            PlayTone(value);
            AddConfirmToolbarItem();

            _selectedTone = value;
            RaisePropertyChanged("SelectedTone");
        }

        private void AddConfirmToolbarItem()
        {
            if (CurrentPage.ToolbarItems.Count == 0)
            {
                CurrentPage.ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Сохранить",
                    Icon = "save",
                    Command = ToneSavedCommand
                });
            }
        }

        /// <summary>
        /// Saves the tone to settings when the tone has been selected
        /// </summary>
        /// <param name="alarmTone">The selected alarm tone</param>
        private void OnToneSelected(AlarmTone alarmTone)
        {
            _alarmStorage.Realm.Write(() => _alarmStorage.GetSettings().AlarmTone = alarmTone);
        }

        /// <summary>
        /// When the user selects an audio file from the file system
        /// </summary>
        /// <param name="uri">URI of the chosen audio file</param>
        private void ToneFileChosen(Uri uri)
        {
            _newToneUri = uri;
            _namingPopupPage = new AlarmToneNamingPopupPage();
            _namingPopupPage.ToneNameSet += OnNewToneNameSet;
            FileNeedsNamed = true;
        }

        /// <summary>
        /// Action to be done when a newly added tone has its name set
        /// </summary>
        /// <param name="toneName">The name to be given to the alarm tone</param>
        private void OnNewToneNameSet(string toneName)
        {
            AlarmTone newTone = new AlarmTone
            {
                Name = toneName,
                Path = _newToneUri.LocalPath,
                IsCustomTone = true
            };

            AllAlarmTones.Add(newTone);
            _alarmStorage.AddTone(newTone);

            _namingPopupPage.ToneNameSet -= OnNewToneNameSet;
            _fileLocator.FileChosen -= ToneFileChosen;
            SetSelectedTone(newTone);
            FileNeedsNamed = false;
        }

        /// <summary>
        /// Deletes the given alarm tone
        /// </summary>
        /// <param name="alarmTone">Alarm tone to delete</param>
        private void DeleteAlarmTone(AlarmTone alarmTone)
        {
            AllAlarmTones.Remove(alarmTone);
            _alarmStorage.Realm.Write(() => _alarmStorage.Realm.Remove(alarmTone));

            _soundService.StopAudio();
        }

        private async void EditAlarmTone(AlarmTone alarmTone)
        {
            _namingPopupPage = new AlarmToneNamingPopupPage(alarmTone);
            await PopupNavigation.Instance.PushAsync(_namingPopupPage);
        }

        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            if (FileNeedsNamed)
            {
                await PopupNavigation.Instance.PushAsync(_namingPopupPage);
            }
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            _soundService.StopAudio();
            base.ViewIsDisappearing(sender, e);
        }
    }
}