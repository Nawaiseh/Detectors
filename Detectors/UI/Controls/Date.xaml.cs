using System;
using System.Windows;
using System.Windows.Controls;

namespace Detectors.UI.Controls
{
    public partial class Date : UserControl
	{
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Dependacy static Variables  ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        public static readonly DependencyProperty YEAR_PROPERTY          = DependencyProperty.Register("DateYear"    , typeof(int     ), typeof(Date), new UIPropertyMetadata(2018           , new PropertyChangedCallback(OnDateChanged        )));
		public static readonly DependencyProperty MONTH_PROPERTY         = DependencyProperty.Register("DateMonth"   , typeof(int     ), typeof(Date), new UIPropertyMetadata(01             , new PropertyChangedCallback(OnDateChanged        )));
		public static readonly DependencyProperty DAY_PROPERTY           = DependencyProperty.Register("DateDay"     , typeof(int     ), typeof(Date), new UIPropertyMetadata(01             , new PropertyChangedCallback(OnDateChanged        )));
		public static readonly DependencyProperty HOURS_PROPERTY         = DependencyProperty.Register("Hours"       , typeof(int     ), typeof(Date), new UIPropertyMetadata(00             , new PropertyChangedCallback(OnTimeChanged        )));
		public static readonly DependencyProperty MINUTES_PROPERTY       = DependencyProperty.Register("Minutes"     , typeof(int     ), typeof(Date), new UIPropertyMetadata(00             , new PropertyChangedCallback(OnTimeChanged        )));
		public static readonly DependencyProperty SECONDS_PROPERTY       = DependencyProperty.Register("Seconds"     , typeof(int     ), typeof(Date), new UIPropertyMetadata(00             , new PropertyChangedCallback(OnTimeChanged        )));
		public static readonly DependencyProperty SELECTED_DATE_PROPERTY = DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(Date), new UIPropertyMetadata(DateTime.Now   , new PropertyChangedCallback(OnSelectedDateChanged)));
		public static readonly DependencyProperty SELECTED_TIME_PROPERTY = DependencyProperty.Register("SelectedTime", typeof(TimeSpan), typeof(Date), new UIPropertyMetadata(new TimeSpan(0), new PropertyChangedCallback(OnSelectedDateChanged)));
		public static readonly DependencyProperty HEADER_PROPERTY        = DependencyProperty.Register("Header"      , typeof(string  ), typeof(Date), new UIPropertyMetadata(""             , new PropertyChangedCallback(OnHeaderChanged      )));

        #endregion
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Depndacy Properities        ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        public int      DateYear     { get => (int     )GetValue(YEAR_PROPERTY         ); set => SetValue(YEAR_PROPERTY         , value); }
		public int      DateMonth    { get => (int     )GetValue(MONTH_PROPERTY        ); set => SetValue(MONTH_PROPERTY        , value); }
		public int      DateDay      { get => (int     )GetValue(DAY_PROPERTY          ); set => SetValue(DAY_PROPERTY          , value); }

		public int      Hours        { get => (int)GetValue(HOURS_PROPERTY             ); set => SetValue(HOURS_PROPERTY        , value); }
		public int      Minutes      { get => (int)GetValue(MINUTES_PROPERTY           ); set => SetValue(MINUTES_PROPERTY      , value); }
		public int      Seconds      { get => (int)GetValue(SECONDS_PROPERTY           ); set => SetValue(SECONDS_PROPERTY      , value); }

		public DateTime SelectedDate { get => (DateTime)GetValue(SELECTED_DATE_PROPERTY); set => SetValue(SELECTED_DATE_PROPERTY, value); }
		public TimeSpan SelectedTime { get => (TimeSpan)GetValue(SELECTED_TIME_PROPERTY); set => SetValue(SELECTED_TIME_PROPERTY, value); }

		public string   Header       { get => (string  )GetValue(HEADER_PROPERTY       ); set => SetValue(HEADER_PROPERTY       , value); }
        #endregion
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Default Constructor         ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        public Date() => InitializeComponent();
        #endregion
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   static Methods              ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        private static void OnTimeChanged(DependencyObject DependencyObject, DependencyPropertyChangedEventArgs eDependencyPropertyChangedEventArgs)
		{
			Date Date = DependencyObject as Date;
			Date.DateTimePicker.SelectedTime = new TimeSpan(Date.Hours, Date.Minutes, Date.Seconds);
		}
		private static void OnDateChanged(DependencyObject DependencyObject, DependencyPropertyChangedEventArgs eDependencyPropertyChangedEventArgs)
		{
			Date Date = DependencyObject as Date;
            Date.DateTimePicker.SelectedDate = new DateTime(Date.DateYear, Date.DateMonth, Date.DateDay);
		}
		private static void OnSelectedDateChanged(DependencyObject DependencyObject, DependencyPropertyChangedEventArgs eDependencyPropertyChangedEventArgs) { }
		private static void OnHeaderChanged(DependencyObject DependencyObject, DependencyPropertyChangedEventArgs eDependencyPropertyChangedEventArgs) { }

        #endregion
    }
}
