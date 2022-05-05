using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.ComponentModel;

namespace Lab1_ClassLibrary
{
    [Serializable]
    public class TeamObservable : ObservableCollection<Person>, INotifyPropertyChanged
    {
        [field:NonSerialized]
        protected override event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        private bool flagChanged;
        public bool FlagChanged
        {
            get
            {
                return flagChanged;
            }
            set
            {
                flagChanged = value;
                OnPropertyChanged("FlagChanged");
                OnPropertyChanged("ResearchersPercent");
            }
        }

        public List<string> subjects;
        public string Name { get; set; }
        public List<string> Subjects
        {
            get
            {
                return subjects;
            }
        }
        private double researchersPercent;
        public double ResearchersPercent
        {
            get
            {
                double r = 0.0;
                foreach (Person p in Items)
                {
                    if (p is Researcher) ++r;
                }
                return r / Count * 100;
            }
        }

        public TeamObservable() { }
        public TeamObservable(string name = "name")
        {
            Name = name;
            flagChanged = false;
            researchersPercent = 0.0;
            subjects = new List<string>();
            subjects.Add("biology");
            subjects.Add("physics");
            subjects.Add("math");
            subjects.Add("chemistry");
            CollectionChanged += updateFlag;
        }

        public void updateFlag(object sender, NotifyCollectionChangedEventArgs e)
        {
            FlagChanged = true;
        }
        public void AddPerson(params Person[] persons)
        {
            foreach (Person new_p in persons)
            {
                bool is_in = false;
                foreach (Person p in Items)
                {
                    if (p == new_p)
                    {
                        is_in = true;
                        break;
                    }
                }
                if (!is_in)
                {
                    base.Add(new_p);
                }
            }
        }

        public void RemovePersonAt(int index)
        {
            Items.RemoveAt(index);
        }

        public Person[] AddDefaults()
        {
            Person[] persons = new Person[6];
            persons[0] = new Person("Alice", "Smith", new DateTime(1990, 10, 10));
            persons[1] = new Programmer("Bob", "Lee", new DateTime(1985, 5, 12), 5, "math");
            persons[2] = new Researcher("Charles", "Jackson", new DateTime(1987, 7, 25), "physics", 0);
            persons[3] = new Person("Dmitry", "Ivanov", new DateTime(1993, 2, 28));
            persons[4] = new Researcher("Ethan", "Carter", new DateTime(1991, 11, 5), "biology", 7);
            persons[5] = new Programmer("Fiona", "Finch", new DateTime(1995, 5, 13), 3, "math");

            AddPerson(persons);
            return persons;
        }

        public Programmer AddDefaultProgrammer()
        {
            Programmer p = new Programmer();
            AddPerson(p);
            return p;
        }

        public Researcher AddDefaultResearcher()
        {
            Researcher r = new Researcher();
            AddPerson(r);
            return r;
        }
        public override string ToString()
        {
            string s = this.Name + '\n';
            s += "Percent of researchers: " + researchersPercent.ToString() + '\n';
            foreach (Person p in Items)
            {
                s += p.ToString() + '\n';
            }
            s += '\n';
            return s;
        }

        public static bool Save(string filename, TeamObservable obj)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
                fs.Close();
                obj.flagChanged = false;
                return true;
            }
            catch (Exception)
            {
                if (fs != null) fs.Close();
                return false;
            }
        }

        public static bool Load(string filename, ref TeamObservable obj)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                obj = bf.Deserialize(fs) as TeamObservable;
                fs.Close();
                obj.flagChanged = false;
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
