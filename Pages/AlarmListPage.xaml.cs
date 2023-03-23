using Xamarin.Forms;

namespace AlarmApp.Pages
{
    public partial class AlarmListPage : ContentPage
    {
        public AlarmListPage()
        {
            InitializeComponent();
        }

        private void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}