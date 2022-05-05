using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace Model
{
    public class ModelDataDraw
    {
        public static List<string> NumberFormats = new List<string> { "F1", "F2", "E1", "E2" };

        public static void Draw(Chart chart, double X, string format, ObservableModelData collection, IList<ModelData> selectedDataModels)
        {
            try
            {
                chart.Series.Clear();
                chart.ChartAreas[0].AxisX.LabelStyle.Format = format;
                chart.ChartAreas[0].AxisY.LabelStyle.Format = format;
                chart.ChartAreas[1].AxisX.LabelStyle.Format = format;
                chart.ChartAreas[1].AxisY.LabelStyle.Format = format;

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
                double[] parameters = new double[collection.Count()];
                foreach (ModelData m in collection) parameters[j++] = m.P;
                double[] meshF = collection.Calculate(X);
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
    }
}
