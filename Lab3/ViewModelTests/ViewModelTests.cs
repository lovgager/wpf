using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewModel;

namespace ViewModelTests
{
    [TestClass]
    public class ViewModelTests
    {
        [TestMethod]
        public void TestSaveLoad()
        {
            MeshFunctionCollection saving = new MeshFunctionCollection(true);
            MeshFunctionCollection loaded = new MeshFunctionCollection();
            string filename = "test";
            saving.Save(filename);
            loaded.Load(filename);
            Assert.AreEqual(saving.ToString(), loaded.ToString());
        }

        [TestMethod]
        public void TestValidation()
        {
            MeshFunctionVisible mfv = new MeshFunctionVisible();
            mfv.X = 2.0;
            Assert.AreNotEqual(mfv["X"], string.Empty);
        }

        [TestMethod]
        public void TestValidation2()
        {
            MeshFunction mf = new MeshFunction();
            mf.P = 11;
            Assert.AreNotEqual(mf.Error, string.Empty);
            mf = new MeshFunction();
            mf.Nodes = 101;
            Assert.AreNotEqual(mf.Error, string.Empty);
        }
    }
}
