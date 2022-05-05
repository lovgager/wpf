using System;

namespace Lab1_ClassLibrary
{
    [Serializable]
    public class Programmer : Person
    {
        public double Exp { get; set; }
        public string Subject { get; set; }

        public Programmer(string firstName = "FirstName", string lastName = "LastName",
                DateTime birthday = new DateTime(), double exp = 0.0, string subject = "math") :
                base(firstName, lastName, birthday)
        {
            Exp = exp;
            Subject = subject;
            if (birthday.Year == 1) Birthday = new DateTime(2000, 1, 1);
        }

        public override string ToString()
        {
            return base.ToString() + " " + Exp + " " + Subject + " - programmer";
        }

        public override object DeepCopy()
        {
            return new Programmer(FirstName, LastName, Birthday, Exp, Subject);
        }
    }
}
