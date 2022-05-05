using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Model
{
    public static class SaveLoad
    {
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
                if (obj == null) return false;
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
