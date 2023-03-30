using System;
using System.Windows.Input;
using Xamarin.Forms;
namespace AlarmApp.Views
{
	public class SettingsViewCell : ViewCell
	{
		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create("Command", typeof(ICommand), typeof(SettingsViewCell), null);

		public ICommand Command
		{
			get => (ICommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		public static readonly BindableProperty CommandParameterProperty =
			BindableProperty.Create("CommandParameter", typeof(object), typeof(SettingsViewCell), null);

		public object CommandParameter
		{
			get => (object)GetValue(CommandParameterProperty);
			set => SetValue(CommandParameterProperty, value);
		}

		protected override void OnAppearing()
		{
			Tapped += OnCellTapped;
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			Tapped -= OnCellTapped;
			base.OnDisappearing();
		}

		private void OnCellTapped(object sender, EventArgs e)
		{
			bool canExecute = Command.CanExecute(CommandParameter);
			if (canExecute)
			{
				Command.Execute(CommandParameter);
			}
		}
	}
}