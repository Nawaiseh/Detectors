using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Detectors.UI.Controls
{
    public partial class Time : UserControl
	{
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Dependacy static Variables  ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        public static readonly DependencyProperty VALUE_PROPERTY   = DependencyProperty.Register("Value"  , typeof(TimeSpan), typeof(Time), new UIPropertyMetadata(DateTime.Now.TimeOfDay, new PropertyChangedCallback(OnValueChanged)));
		public static readonly DependencyProperty HOURS_PROPERTY   = DependencyProperty.Register("Hours"  , typeof(int     ), typeof(Time), new UIPropertyMetadata(0                     , new PropertyChangedCallback(OnTimeChanged )));
		public static readonly DependencyProperty MINUTES_PROPERTY = DependencyProperty.Register("Minutes", typeof(int     ), typeof(Time), new UIPropertyMetadata(0                     , new PropertyChangedCallback(OnTimeChanged )));
		public static readonly DependencyProperty SECONDS_PROPERTY = DependencyProperty.Register("Seconds", typeof(int     ), typeof(Time), new UIPropertyMetadata(0                     , new PropertyChangedCallback(OnTimeChanged )));

        #endregion
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Depndacy Properities        ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        public int      Hours   { get => (int     )GetValue(HOURS_PROPERTY  ); set => SetValue(HOURS_PROPERTY  , value); }
		public int      Minutes { get => (int     )GetValue(MINUTES_PROPERTY); set => SetValue(MINUTES_PROPERTY, value); }
		public int      Seconds { get => (int     )GetValue(SECONDS_PROPERTY); set => SetValue(SECONDS_PROPERTY, value); }

        public TimeSpan Value   { get => (TimeSpan)GetValue(VALUE_PROPERTY  ); set => SetValue(VALUE_PROPERTY  , value); }
        #endregion
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Default Constructor         ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        public Time() => InitializeComponent();

        #endregion
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   static Methods              ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

		private static void OnValueChanged(DependencyObject DependencyObject, DependencyPropertyChangedEventArgs DependencyPropertyChangedEventArgs)
		{
			TimeSpan NewValue     = (TimeSpan)DependencyPropertyChangedEventArgs.NewValue;
			Time TimePicker = DependencyObject as Time;
			TimePicker.Hours      = NewValue.Hours;
			TimePicker.Minutes    = NewValue.Minutes;
			TimePicker.Seconds    = NewValue.Seconds;
		}

		private static void OnTimeChanged(DependencyObject DependencyObject, DependencyPropertyChangedEventArgs eDependencyPropertyChangedEventArgs)
		{
			Time TimePicker = DependencyObject as Time;
			TimePicker.Value      = new TimeSpan(TimePicker.Hours, TimePicker.Minutes, TimePicker.Seconds);
		}

        #endregion
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Events Methods              ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        private void TextBox_MouseWheel(object Sender, MouseWheelEventArgs MouseWheelEventArgs) => Scroll((TextBox)Sender, MouseWheelEventArgs.Delta, int.Parse((string)((TextBox)Sender).Tag));

        private void TextBox_KeyDown(object Sender, KeyEventArgs KeyEventArgs)
		{
			Key Key = KeyEventArgs.Key;
			if (Key != Key.Up && Key != Key.Down) { return; }
			TextBox TextBox = (TextBox)Sender;
			int Delta = (Key == Key.Up) ? 1 : -1;
			Scroll(TextBox, Delta, int.Parse((string)((TextBox)Sender).Tag));
		}

		private void Scroll(TextBox TextBox, int Delta, int Maximum = 59)
		{
			int TimePortion = int.Parse(TextBox.Text);
			TimePortion = (Delta < 0)
				? (TimePortion <= 0) ? Maximum : (TimePortion - 1)
				: (TimePortion >= Maximum) ? 00 : (TimePortion + 1);

			TextBox.Text = TimePortion.ToString("00");
			TextBox.Select(0, 2);
		}

        #endregion
    }
}

