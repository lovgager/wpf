using System;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.IO;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lab2_ClassLibrary
{
    [Serializable]
    public class ObservableModelData : ObservableCollection<ModelData>, INotifyPropertyChanged
    {
        public bool ChangesUnsaved { get; set; }
        
        public void Add_ModelData(ModelData modelData)
        {
            Add(modelData);
        }

        public void Remove_At(int index)
        {
            RemoveAt(index);
        }

        public void AddDefaults()
        {
            Add(new ModelData(0, 3));
            Add(new ModelData(1, 3));
            Add(new ModelData(1, 10));
            Add(new ModelData(2, 3));
            Add(new ModelData(3, 10));
            Add(new ModelData(5, 200));
        }

        public ObservableModelData()
        {
            AddDefaults();
            CollectionChanged += updateFlag;
            ChangesUnsaved = false;
        }

        public void updateFlag(object sender, NotifyCollectionChangedEventArgs e)
        {
            ChangesUnsaved = true;
        }

        public double[] Calculate(double x)
        {
            double[] values = new double[Count];
            for (int i = 0; i < Count; ++i) values[i] = this[i].F(x);
            return values;
        }

        public override string ToString()
        {
            string s = string.Empty;
            foreach (ModelData md in this)
                s += "Nodes : " + md.Nodes.ToString() + " Parameter: " + md.P.ToString() + '\n';
            return s;
        }

        public static bool Save(string filename, ObservableModelData obj)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
                fs.Close();
                obj.ChangesUnsaved = false;
                return true;
            }
            catch (Exception)
            {
                if (fs != null) fs.Close();
                return false;
            }
        }

        public static bool Load(string filename, ref ObservableModelData obj)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                obj = bf.Deserialize(fs) as ObservableModelData;
                fs.Close();
                obj.ChangesUnsaved = false;
                return true;
            }
            catch (Exception)
            {
                if (fs != null) fs.Close();
                return false;
            }
        }
    }
}
