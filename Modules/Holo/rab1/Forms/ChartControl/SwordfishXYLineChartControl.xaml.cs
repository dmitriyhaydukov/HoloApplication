using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//using Swordfish.WPF.Charts;
using Swordfish.NET.Charts;

namespace rab1 {
    /// <summary>
    /// Логика взаимодействия для SwordfishXYLineControl.xaml
    /// </summary>
    public partial class SwordfishXYLineChartControl : UserControl {
        //-------------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty GraphInfoCollectionProperty;
        public static readonly DependencyProperty AxesInfoProperty;
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        public AxesInfo GraphAxesInfo {
            get {
                return ( AxesInfo )this.GetValue( SwordfishXYLineChartControl.AxesInfoProperty );
            }
            set {
                this.SetValue( SwordfishXYLineChartControl.AxesInfoProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        public IList<GraphInfo> GraphInfoCollection {
            get {
                return ( IList<GraphInfo> )this.GetValue( SwordfishXYLineChartControl.GraphInfoCollectionProperty );
            }
            set {
                this.SetValue( SwordfishXYLineChartControl.GraphInfoCollectionProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        static SwordfishXYLineChartControl() {
            DependencyPropertyInfo info;
            info = new DependencyPropertyInfo();
            info.PropertyName = "GraphInfoCollection";
            info.DataType = typeof( IList<GraphInfo> );
            info.OwnerDataType = typeof( SwordfishXYLineChartControl );
            info.PropertyChangedHandler = 
                new PropertyChangedCallback( SwordfishXYLineChartControl.GraphInfoCollectionChanged );
            SwordfishXYLineChartControl.GraphInfoCollectionProperty = ExtraHelperWPF.RegisterDependencyProperty( info );

            info = new DependencyPropertyInfo();
            info.PropertyName = "GraphAxesInfo";
            info.DataType = typeof( AxesInfo );
            info.OwnerDataType = typeof( SwordfishXYLineChartControl );
            info.PropertyChangedHandler =
                new PropertyChangedCallback( SwordfishXYLineChartControl.AxesInfoChanged );
            SwordfishXYLineChartControl.AxesInfoProperty = ExtraHelperWPF.RegisterDependencyProperty( info );
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        public SwordfishXYLineChartControl() {
            InitializeComponent();
            this.SetDefaultSettings();
            this.SetHandlers();
        }
        //-------------------------------------------------------------------------------------------------------
        private void SetHandlers()
        {
            this.redrawChartButton.Click += RedrawChartButton_Click;
        }
        //-------------------------------------------------------------------------------------------------------
        private void RedrawChart()
        {
            ChartPrimitive chartPrimitive = this.xyLineChart.Primitives.FirstOrDefault();
            if (chartPrimitive != null)
            {
                ChartPrimitiveXY chartPrimitiveXY = chartPrimitive as ChartPrimitiveXY;
                if (chartPrimitiveXY != null)
                {
                    chartPrimitiveXY.ShowPoints = this.showPointsCheckBox.IsChecked.Value;
                    chartPrimitiveXY.PointColor = Colors.Red;

                    chartPrimitive.IsDashed = this.dashCheckBox.IsChecked.Value;
                                        
                    if (this.showLinesCheckBox.IsChecked.Value)
                    {
                        chartPrimitiveXY.LineThickness = 1;
                    }
                    else
                    {
                        chartPrimitiveXY.LineThickness = 0;
                    }

                    this.xyLineChart.RedrawPlotLines();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        private void RedrawChartButton_Click(object sender, RoutedEventArgs e)
        {
            RedrawChart();
        }
        //-------------------------------------------------------------------------------------------------------
        //Настройки по умолчанию
        private void SetDefaultSettings() {
            this.Title = "Graphic";

            this.showLinesCheckBox.IsChecked = true;
            this.showPointsCheckBox.IsChecked = false;

            //this.xyLineChart.XAxisLabel = "X";
            //this.xyLineChart.YAxisLabel = "Y";
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //Заголовок
        public string Title {
            get {
                return this.xyLineChart.Title;
            }
            set {
                this.xyLineChart.Title = value;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //Нарисовать график
        private void DrawChart
        ( 
            double[] xValues,
            double[] yValues,
            Color color,
            string chartName,
            bool isLineVisible,
            bool isPointsVisible,
            bool isClean 
        ) {
            if ( isClean ) {
                this.xyLineChart.Reset();
            }

            ChartPrimitiveXY chartPrimitive = this.xyLineChart.CreateXY();
            chartPrimitive.LegendColor = color;
            chartPrimitive.LineColor = color;
            chartPrimitive.Label = chartName;
            chartPrimitive.LineThickness = isLineVisible ? 1 : 0;
            chartPrimitive.ShowPoints = isPointsVisible;

            for ( int index = 0; index < xValues.Length; index++ ) {
                double x = xValues[ index ];
                double y = yValues[ index ];
                chartPrimitive.AddPoint( x, y );
            }
            this.xyLineChart.AddPrimitive(chartPrimitive);
            this.xyLineChart.RedrawPlotLines();
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        private static void GraphInfoCollectionChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            SwordfishXYLineChartControl chartControl = d as SwordfishXYLineChartControl;
            IList<GraphInfo> graphInfoCollection = ( IList<GraphInfo> )e.NewValue;
            if ( graphInfoCollection == null ) {
                return;
            }
                        
            chartControl.xyLineChart.Reset();

            for ( int index = 0; index < graphInfoCollection.Count; index++ ) {
                GraphInfo graphInfo = graphInfoCollection[ index ];
                double[] xValues = PlaneManager.GetCoordinatesX( graphInfo.GraphPoints );
                double[] yValues = PlaneManager.GetCoordinatesY( graphInfo.GraphPoints );

                chartControl.DrawChart
                    ( xValues, yValues, graphInfo.GraphColor, graphInfo.GraphName, graphInfo.LineVisibility, graphInfo.PointsVisibility, false );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        public static void AxesInfoChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            SwordfishXYLineChartControl chartControl = d as SwordfishXYLineChartControl;
            AxesInfo axesInfo = ( AxesInfo )e.NewValue;
            if ( axesInfo == null ) {
                return;
            }

            //chartControl.xyLineChart.XAxisLabel = axesInfo.AxisTitleX;
            //chartControl.xyLineChart.YAxisLabel = axesInfo.AxisTitleY;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
    }
}
