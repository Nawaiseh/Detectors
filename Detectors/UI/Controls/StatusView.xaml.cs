using System.Windows;
using System.Windows.Controls.Primitives;

namespace Detectors.UI.Controls
{
    public partial class StatusView : StatusBar
	{
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Dependacy static Variables  ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        public static readonly DependencyProperty TOTAL_DAYS_PROPERTY              = DependencyProperty.Register("TotalDays"              , typeof(int   ), typeof(StatusView), new UIPropertyMetadata(0          , new PropertyChangedCallback(OnValueChangedChanged)));
		public static readonly DependencyProperty REMAINING_DAYS_PROPERTY          = DependencyProperty.Register("RemainingDays"          , typeof(int   ), typeof(StatusView), new UIPropertyMetadata(0          , new PropertyChangedCallback(OnValueChangedChanged)));
		public static readonly DependencyProperty TOTAL_RECORDS_PROPERTY           = DependencyProperty.Register("TotalRecords"           , typeof(int   ), typeof(StatusView), new UIPropertyMetadata(0          , new PropertyChangedCallback(OnValueChangedChanged)));
		public static readonly DependencyProperty REMAINING_RECORDS_PROPERTY       = DependencyProperty.Register("RemainingRecords"       , typeof(int   ), typeof(StatusView), new UIPropertyMetadata(0          , new PropertyChangedCallback(OnValueChangedChanged)));
		public static readonly DependencyProperty FAILED_RECORDS_PROPERTY          = DependencyProperty.Register("FailedRecords"          , typeof(int   ), typeof(StatusView), new UIPropertyMetadata(0          , new PropertyChangedCallback(OnValueChangedChanged)));
		public static readonly DependencyProperty PROGRESS_PROPERTY                = DependencyProperty.Register("Progress"               , typeof(int   ), typeof(StatusView), new UIPropertyMetadata(0          , new PropertyChangedCallback(OnValueChangedChanged)));
		public static readonly DependencyProperty MAXIMUM_PROPERTY                 = DependencyProperty.Register("Maximum"                , typeof(int   ), typeof(StatusView), new UIPropertyMetadata(0          , new PropertyChangedCallback(OnValueChangedChanged)));
		public static readonly DependencyProperty MINIMUM_PROPERTY                 = DependencyProperty.Register("Minimum"                , typeof(int   ), typeof(StatusView), new UIPropertyMetadata(0          , new PropertyChangedCallback(OnValueChangedChanged)));
		public static readonly DependencyProperty EXECUTION_TIME_PROPERTY          = DependencyProperty.Register("ExecutionTime"          , typeof(string), typeof(StatusView), new UIPropertyMetadata(""         , new PropertyChangedCallback(OnValueChangedChanged)));

		public static readonly DependencyProperty TOTAL_DAYS_HEADER_PROPERTY        = DependencyProperty.Register("TotalDaysHeader"       , typeof(string), typeof(StatusView), new UIPropertyMetadata("Total"    , new PropertyChangedCallback(OnTextChangedChanged)));
		public static readonly DependencyProperty REMAINING_DAYS_HEADER_PROPERTY    = DependencyProperty.Register("RemainingDaysHeader"   , typeof(string), typeof(StatusView), new UIPropertyMetadata("Remaining", new PropertyChangedCallback(OnTextChangedChanged)));
		public static readonly DependencyProperty TOTAL_RECORDS_HEADER_PROPERTY     = DependencyProperty.Register("TotalRecordsHeader"    , typeof(string), typeof(StatusView), new UIPropertyMetadata("Total"    , new PropertyChangedCallback(OnTextChangedChanged)));
		public static readonly DependencyProperty REMAINING_RECORDS_HEADER_PROPERTY = DependencyProperty.Register("RemainingRecordsHeader", typeof(string), typeof(StatusView), new UIPropertyMetadata("Remaining", new PropertyChangedCallback(OnTextChangedChanged)));
		public static readonly DependencyProperty FAILED_RECORDS_HEADER_PROPERTY    = DependencyProperty.Register("FailedRecordsHeader"   , typeof(string), typeof(StatusView), new UIPropertyMetadata("Remaining", new PropertyChangedCallback(OnTextChangedChanged)));

        #endregion
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Depndacy Properities        ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        public int    TotalDays              { get => (int   )GetValue(TOTAL_DAYS_PROPERTY              ); set => SetValue(TOTAL_DAYS_PROPERTY              , value); }
		public int    RemainingDays          { get => (int   )GetValue(REMAINING_DAYS_PROPERTY          ); set => SetValue(REMAINING_DAYS_PROPERTY          , value); }
		public int    TotalRecords           { get => (int   )GetValue(TOTAL_RECORDS_PROPERTY           ); set => SetValue(TOTAL_RECORDS_PROPERTY           , value); }
		public int    RemainingRecords       { get => (int   )GetValue(REMAINING_RECORDS_PROPERTY       ); set => SetValue(REMAINING_RECORDS_PROPERTY       , value); }
		public int    FailedRecords          { get => (int   )GetValue(FAILED_RECORDS_PROPERTY          ); set => SetValue(FAILED_RECORDS_PROPERTY          , value); }
		public int    Progress               { get => (int   )GetValue(PROGRESS_PROPERTY                ); set => SetValue(PROGRESS_PROPERTY                , value); }
		public int    Maximum                { get => (int   )GetValue(MAXIMUM_PROPERTY                 ); set => SetValue(MAXIMUM_PROPERTY                 , value); }
		public int    Minimum                { get => (int   )GetValue(MINIMUM_PROPERTY                 ); set => SetValue(MINIMUM_PROPERTY                 , value); }
		public string ExecutionTime          { get => (string)GetValue(EXECUTION_TIME_PROPERTY          ); set => SetValue(EXECUTION_TIME_PROPERTY          , value); }

		public string TotalDaysHeader        { get => (string)GetValue(TOTAL_DAYS_HEADER_PROPERTY       ); set => SetValue(TOTAL_DAYS_HEADER_PROPERTY       , value); }
		public string RemainingDaysHeader    { get => (string)GetValue(REMAINING_DAYS_HEADER_PROPERTY   ); set => SetValue(REMAINING_DAYS_HEADER_PROPERTY   , value); }
		public string TotalRecordsHeader     { get => (string)GetValue(TOTAL_RECORDS_HEADER_PROPERTY    ); set => SetValue(TOTAL_RECORDS_HEADER_PROPERTY    , value); }
		public string RemainingRecordsHeader { get => (string)GetValue(REMAINING_RECORDS_HEADER_PROPERTY); set => SetValue(REMAINING_RECORDS_HEADER_PROPERTY, value); }
		public string FailedRecordsHeader    { get => (string)GetValue(FAILED_RECORDS_HEADER_PROPERTY   ); set => SetValue(FAILED_RECORDS_HEADER_PROPERTY   , value); }

        #endregion
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Default Constructor         ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        public StatusView() => InitializeComponent();

        #endregion
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   static Methods              ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        private static void OnTextChangedChanged(DependencyObject DependencyObject, DependencyPropertyChangedEventArgs eDependencyPropertyChangedEventArgs) { }
		private static void OnValueChangedChanged(DependencyObject DependencyObject, DependencyPropertyChangedEventArgs eDependencyPropertyChangedEventArgs) { }

        #endregion
    }
}
