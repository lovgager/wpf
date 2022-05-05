using System;

namespace Lab1_ClassLibrary
{
    [Serializable]
    public class Researcher : Person, IComparable<Researcher>
    {
        public string Subject { get; set; }
        public int Publications { get; set; }

        public Researcher(string firstname = "FirstName", string lastname = "LastName", 
                DateTime birthday = new DateTime(), string subject = "math", int publications = 0)
            : base(firstname, lastname, birthday)
        {
            Subject = subject;
            Publications = publications;
            if (birthday.Year == 1) Birthday = new DateTime(2000, 1, 1);
        }

        public override string ToString()
        {
            return base.ToString() + " " + Subject + " " + Publications + " - researcher";
        }

        public override object DeepCopy()
        {
            Researcher r = new Researcher(FirstName, LastName, Birthday, Subject, Publications);
            return r;
        }

        public int CompareTo(Researcher other)
        {
            return Publications.CompareTo(other.Publications);
        }
    }
}
