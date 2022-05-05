using System.Collections.Generic;
using Model;
using System.Windows.Forms.DataVisualization.Charting;
using System.ComponentModel;
using System.Collections;

namespace ViewModel
{
    public class MeshFunctionVisible : IDataErrorInfo
    {
        public double X { get; set; }

        public string NumberFormat { get; set; }
        public List<string> NumberFormats { get { return ModelDataDraw.NumberFormats; } }

        public MeshFunctionVisible()
        {
            X = 0.0;
            NumberFormat = "F1";
        }

        public void Draw(Chart chart, MeshFunctionCollection mfc, IList selected)
        {
            List<ModelData> selectedModelData = new List<ModelData>();
            foreach (var item in selected)
                selectedModelData.Add(item as ModelData);
            ModelDataDraw.Draw(chart, X, NumberFormat, mfc.Collection, selectedModelData);
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
            get { return this["X"]; }
        }
    }
}
