using System;

namespace Lab1_ClassLibrary
{
    [Serializable]
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }

        public string BirthdayShort { 
            get
            {
                return Birthday.ToShortDateString();
            } 
        }

        public Person(string firstName, string lastName, DateTime birthday)
        {
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
        }

        public Person ThisPerson { get { return this; } }

        public override string ToString()
        {
            return FirstName + " " + LastName + " " + Birthday.ToString("d");
        }

        public virtual object DeepCopy()
        {
            Person p = new Person(FirstName, LastName, Birthday);
            return p;
        }
    }
}
