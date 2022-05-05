using Model;
using System.ComponentModel;

namespace ViewModel
{
    public class MeshFunction : IDataErrorInfo
    {
        public double P { get; set; }
        public int Nodes { get; set; }

        public MeshFunction()
        {
            P = 0.0;
            Nodes = 3;
        }
        
        public ModelData toModelData()
        {
            ModelData md = new ModelData();
            md.P = P;
            md.Nodes = Nodes;
            return md;
        }

        public string this[string columnName]
        {
            get { return (toModelData())[columnName]; }
        }

        public string Error
        {
            get { return (toModelData()).Error; }
        }
    }
}
