using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.ComponentModel;

namespace Lab2_ClassLibrary
{
    public class ModelDataView : IDataErrorInfo
    {
        public double X { get; set; }
        public string NumbersFormat { get; set; }
        public ObservableModelData myModelDataCollection;

        public ModelDataView(ObservableModelData omd)
        {
            X = 0.0;
            NumbersFormat = "F1";
            myModelDataCollection = omd;
        }

        public void Draw(Chart chart, IList<ModelData> selectedDataModels)
        {
            try
            {
                chart.Series.Clear();
                chart.ChartAreas[0].AxisX.LabelStyle.Format = NumbersFormat;
                chart.ChartAreas[0].AxisY.LabelStyle.Format = NumbersFormat;
                chart.ChartAreas[1].AxisX.LabelStyle.Format = NumbersFormat;
                chart.ChartAreas[1].AxisY.LabelStyle.Format = NumbersFormat;

                int i = 0;
                foreach (ModelData m in selectedDataModels)
                {
                    chart.Series.Add("Series" + i);
                    chart.Series[i].Points.DataBindXY(m.MeshX, m.MeshY);
                    chart.Series[i].ChartType = SeriesChartType.Line;
                    chart.Series[i].BorderWidth = 3;
                    chart.Series[i].LegendText = "p = " + m.P;
                    ++i;
                }

                int j = 0;
                double[] parameters = new double[myModelDataCollection.Count()];
                foreach (ModelData m in myModelDataCollection) parameters[j++] = m.P;
                double[] meshF = myModelDataCollection.Calculate(X);
                chart.Series.Add("Series" + i);
                chart.Series[i].Points.DataBindXY(parameters, meshF);
                chart.Series[i].ChartType = SeriesChartType.Line;
                chart.Series[i].MarkerStyle = MarkerStyle.Circle;
                chart.Series[i].MarkerSize = 7;
                chart.Series[i].BorderWidth = 3;
                chart.Series[i].ChartArea = "area2";
                chart.Series[i].IsVisibleInLegend = false;
                foreach (DataPoint dp in chart.Series[i].Points)
                    dp.ToolTip = "p = " + dp.XValue + ", f = " + dp.YValues[0];
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "X" && (X < 0 || X > 1))
                    return "wrong X";
                return string.Empty;
            }
        }

        public string Error
        {
            get
            {
                return this["X"];
            }
        }
    }
}
