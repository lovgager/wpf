using System;
using System.ComponentModel;

namespace Model
{
    [Serializable]
    public class ModelData : IDataErrorInfo
    {
        private double p;
        public double P
        {
            get { return p; }
            set { p = value; buildMesh(); }
        }
        private int nodes;
        public int Nodes
        {
            get { return nodes; }
            set
            {
                nodes = value;
                MeshX = new double[nodes];
                MeshY = new double[nodes];
                buildMesh();
            }
        }
        public double[] MeshX { get; set; }
        public double[] MeshY { get; set; }

        public static double pMin = 0, pMax = 10;
        public static int nMin = 3, nMax = 100;

        const double EPS = 0.001;

        public ModelData(double p = 0, int nodes = 3)
        {
            Nodes = nodes;
            P = p;
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Nodes":
                        if (Nodes < nMin || nMax < Nodes)
                            return "wrong number of nodes";
                        break;
                    case "P":
                        if (P < pMin || pMax < P)
                            return "wrong parameter";
                        break;
                }
                return string.Empty;
            }
        }

        public string Error
        {
            get
            {
                string s = this["Nodes"];
                if (s != string.Empty) return s;
                return this["P"];
            }
        }

        public double F(double x)
        {
            int i = 0;
            while (MeshX[i + 1] < x) ++i;
            if (x - MeshX[i] < EPS) return analyticF(x);
            double x1 = MeshX[i], x2 = MeshX[i + 1];
            double y1 = analyticF(x1), y2 = analyticF(x2);
            double slope = (y2 - y1) / (x2 - x1);
            double bias = y1 - slope * x1;
            return slope * x + bias;
        }

        public double analyticF(double x)
        {
            return Math.Sin(P * x);
        }

        public void buildMesh()
        {
            double step = 1 / ((double)Nodes - 1);
            for (int i = 0; i < Nodes; ++i)
            {
                MeshX[i] = i * step;
                MeshY[i] = analyticF(MeshX[i]);
            }
        }

        public override string ToString()
        {
            return "Parameter: " + P + "; Nodes: " + Nodes;
        }
    }

}
