using System;
using System.Windows;
using System.Windows.Controls;

namespace Detectors.UI.Controls
{
    public partial class DownloadOptions
	{
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Dependacy static Variables  ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        public static readonly DependencyProperty SPEED_PROPERTY     = DependencyProperty.Register("Speed"    , typeof(bool), typeof(DownloadOptions), new UIPropertyMetadata(false , new PropertyChangedCallback(OnOptionChanged)));
		public static readonly DependencyProperty FLOW_PROPERTY      = DependencyProperty.Register("Flow"     , typeof(bool), typeof(DownloadOptions), new UIPropertyMetadata(false , new PropertyChangedCallback(OnOptionChanged)));
		public static readonly DependencyProperty OCCUPANCY_PROPERTY = DependencyProperty.Register("Occupancy", typeof(bool), typeof(DownloadOptions), new UIPropertyMetadata(false , new PropertyChangedCallback(OnOptionChanged)));
		public static readonly DependencyProperty ALL_PROPERTY       = DependencyProperty.Register("All"      , typeof(bool), typeof(DownloadOptions), new UIPropertyMetadata(true  , new PropertyChangedCallback(OnOptionChanged)));

        #endregion
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Depndacy Properities        ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        public bool Speed     { get => (bool)GetValue(SPEED_PROPERTY    ); set => SetValue(SPEED_PROPERTY    , value); }
        public bool Flow      { get => (bool)GetValue(FLOW_PROPERTY     ); set => SetValue(FLOW_PROPERTY     , value); }
        public bool Occupancy { get => (bool)GetValue(OCCUPANCY_PROPERTY); set => SetValue(OCCUPANCY_PROPERTY, value); }
        public bool All       { get => (bool)GetValue(ALL_PROPERTY      ); set => SetValue(ALL_PROPERTY      , value); }

        #endregion
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Default Constructor         ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        public DownloadOptions() => InitializeComponent();
        #endregion
        #region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   static Methods              ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		private static void OnOptionChanged(DependencyObject DependencyObject, DependencyPropertyChangedEventArgs eDependencyPropertyChangedEventArgs) { }

        #endregion
    }
}
