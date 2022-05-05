using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using Microsoft.Win32;
using Lab1_ClassLibrary;
using System;
using System.Globalization;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private TeamObservable myTeam = new TeamObservable("my team");
        private Researcher customRes = new Researcher();

        void BindResearcher()
        {
            Binding b = new Binding();
            b.Source = customRes;

            b.Path = new PropertyPath("FirstName");
            textBox_resFirstName.SetBinding(TextBox.TextProperty, b);

            b.Path.Path = "LastName";
            textBox_resLastName.SetBinding(TextBox.TextProperty, b);

            b.Path.Path = "Birthday";
            datePicker.SetBinding(DatePicker.SelectedDateProperty, b);

            b.Path.Path = "Subjects";
            comboBox.SetBinding(ComboBox.SelectedValueProperty, b);

            b.Path.Path = "Publications";
            textBox_resPublications.SetBinding(TextBox.TextProperty, b);
        }

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                this.DataContext = myTeam;

                BindResearcher();
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            if (myTeam.FlagChanged)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "", MessageBoxButton.YesNoCancel);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        SaveFileDialog saveDialog = new SaveFileDialog();
                        if ((bool)saveDialog.ShowDialog())
                        {
                            TeamObservable.Save(saveDialog.FileName, myTeam);
                        }
                        myTeam.Clear();
                        break;
                    case MessageBoxResult.No:
                        myTeam.Clear();
                        break;
                }
            }
            else
            {
                myTeam.Clear();
            }
            myTeam.FlagChanged = false;
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            if ((bool)saveDialog.ShowDialog())
            {
                TeamObservable.Save(saveDialog.FileName, myTeam);
                myTeam.FlagChanged = false;
            }

        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if ((bool)openDialog.ShowDialog())
            {
                if (myTeam.FlagChanged)
                {
                    MessageBoxResult result = MessageBox.Show("Do you want to save  changes?", "", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        SaveFileDialog saveDialog = new SaveFileDialog();
                        if ((bool)saveDialog.ShowDialog())
                        {
                            TeamObservable.Save(saveDialog.FileName, myTeam);
                        }

                    }
                }
                if (TeamObservable.Load(openDialog.FileName, ref myTeam))
                {
                    myTeam.FlagChanged = false;
                    DataContext = myTeam;
                    myTeam.CollectionChanged += myTeam.updateFlag;
                }
                else
                {
                    MessageBox.Show("Error");
                }

            }
        }

        private void DefRes_Click(object sender, RoutedEventArgs e)
        {
            myTeam.AddDefaultResearcher();
        }

        private void DefProg_Click(object sender, RoutedEventArgs e)
        {
            myTeam.AddDefaultProgrammer();
        }

        private void Def_Click(object sender, RoutedEventArgs e)
        {
            myTeam.AddDefaults();
        }

        private void CustomRes_Click(object sender, RoutedEventArgs e)
        {
            myTeam.Add(customRes);
            customRes = new Researcher();
            BindResearcher();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            int selected = listBox1.SelectedIndex;
            if (selected >= 0 && selected < myTeam.Count)
            {
                myTeam.RemoveAt(selected);
            }
            else
            {
                MessageBox.Show("Nothing Selected");
            }
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            if (myTeam.FlagChanged)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    if ((bool)saveDialog.ShowDialog())
                    {
                        TeamObservable.Save(saveDialog.FileName, myTeam);
                        myTeam.FlagChanged = false;
                    };
                }
            }
        }

        private void radioButton_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dt = TryFindResource("key_dataTemplate") as DataTemplate;
            if (dt != null)
            {
                listBox1.ItemTemplate = dt;
            }
        }

        private void radioButton1_Click(object sender, RoutedEventArgs e)
        {
            listBox1.ItemTemplate = null;
        }

        public void filterResearchers(object source, FilterEventArgs args)
        {
            Person person = args.Item as Person;
            try
            {
                if (person == null) throw new Exception();
                else args.Accepted = person is Researcher;
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }
        }
    }



    public class PersonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Person p = (Person)value;
            string s = p.FirstName + " " + p.LastName + " ";
            if (p is Programmer) s += "programmer";
            else if (p is Researcher) s += "researcher";
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class ResConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Researcher r = value as Researcher;
            if (r.FirstName == "" || r.LastName == "")
            {
                r.FirstName = "FirstName";
                r.LastName = "LastName";
            }
            return r.LastName + " " + r.FirstName[0];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
