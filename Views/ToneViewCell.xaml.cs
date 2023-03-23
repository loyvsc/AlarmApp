using Xamarin.Forms;

namespace AlarmApp.Views
{
    public partial class ToneViewCell : ViewCell
    {
        public ToneViewCell()
        {
            InitializeComponent();
        }

        private void Handle_BindingContextChanged(object sender, System.EventArgs e)
        {
            base.OnBindingContextChanged();

        }
    }
}