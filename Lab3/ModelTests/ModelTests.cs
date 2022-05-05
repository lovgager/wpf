using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace ModelTests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void TestMesh()
        {
            ModelData md = new ModelData(1, 101);
            Assert.AreEqual(md.MeshX[50], 0.5, 1e-10);
            Assert.AreEqual(Math.Sin(md.MeshX[50]), md.MeshY[50], 1e-10);
        }

        [TestMethod]
        public void TestCalc()
        {
            ObservableModelData omd = new ObservableModelData();
            omd.Clear();
            omd.Add(new ModelData(0, 3));
            omd.Add(new ModelData(1, 3));
            omd.Add(new ModelData(5, 101));

            double x = 0.5;
            double[] values = new double[omd.Count];
            values[0] = 0.0;
            values[1] = Math.Sin(x);
            values[2] = Math.Sin(5*x);

            for (int i = 0; i < omd.Count; ++i) 
                Assert.AreEqual(values[i], omd.Calculate(x)[i], 1e-10);
        }

        [TestMethod]
        public void TestSaveLoad()
        {
            ObservableModelData saving = new ObservableModelData();
            ObservableModelData loaded = new ObservableModelData();
            string filename = "test";
            SaveLoad.Save(filename, saving);
            loaded.Clear();
            SaveLoad.Load(filename, ref loaded);

            Assert.AreEqual(saving.ToString(), loaded.ToString());
        }
    }
}
