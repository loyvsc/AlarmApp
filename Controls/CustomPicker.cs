using System;
using Xamarin.Forms;

namespace AlarmApp.Controls
{
	public class CustomPicker : Picker
	{
		public static readonly BindableProperty HintProperty
			= BindableProperty.Create("Hint", typeof(string), typeof(CustomPicker), null);

		public string Hint 
		{ 
			get => (string)GetValue(HintProperty);
            set => SetValue(HintProperty, value);
		}

		public static readonly BindableProperty IsValidProperty = BindableProperty.Create("IsValid", typeof(bool?), typeof(CustomPicker), null, propertyChanged: OnIsValidChanged);

		public bool? IsValid
		{
            get => (bool?)GetValue(IsValidProperty);
            set => SetValue(IsValidProperty, value);
		}

		public event EventHandler IsValidChanged;

		private static void OnIsValidChanged(BindableObject bindable, object oldValue, object newValue)
		{
            CustomPicker picker = (CustomPicker)bindable;
			picker.IsValidChanged?.Invoke(picker, null);
		}
	}
}