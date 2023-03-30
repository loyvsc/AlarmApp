using System;
using Xamarin.Forms;
namespace AlarmApp.Controls
{
	public class CustomEntry : Entry
    {
        public event EventHandler IsValidChanged;
        
		public static readonly BindableProperty IsValidProperty =  
			BindableProperty.Create("IsValid", typeof(bool?), typeof(CustomEntry), null, propertyChanged: OnIsValidChanged);

		public bool? IsValid
		{
			get => (bool?)GetValue(IsValidProperty);
			set => SetValue(IsValidProperty, value);
		}

		static void OnIsValidChanged(BindableObject bindable, object oldValue, object newValue)
		{
			CustomEntry entry = (CustomEntry)bindable;
			entry.IsValidChanged?.Invoke(entry, null);
		}
	}
}