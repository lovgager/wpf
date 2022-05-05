using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Win32;
using Lab2_ClassLibrary;

namespace Lab2
{
    public partial class MainWindow : Window
    {
        private static ObservableModelData myModelDataCollection = new ObservableModelData();
        private ModelDataView mdv = new ModelDataView(myModelDataCollection);
        private ModelData customMd = new ModelData();
        private Chart chart = new Chart();
        public static RoutedCommand AddModelCommand = new RoutedCommand("Add", typeof(Lab2.MainWindow));
        public static RoutedCommand DrawCommand = new RoutedCommand("Draw", typeof(Lab2.MainWindow));

        private void BindCustomModelData()
        {
            Binding b = new Binding();
            b.ValidatesOnDataErrors = true;
            b.Source = customMd;
            b.Path = new PropertyPath("P");
            textBox_parameter.SetBinding(TextBox.TextProperty, b);
            b.Path.Path = "Nodes";
            textBox_nodes.SetBinding(TextBox.TextProperty, b);
        }

        private void BindModelDataView()
        {
            Binding b = new Binding();
            b.ValidatesOnDataErrors = true;
            b.Source = mdv;
            b.Path = new PropertyPath("X");
            textBox_xValue.SetBinding(TextBox.TextProperty, b);
        }

        private void BindFormat()
        {
            comboBox_format.ItemsSource = new List<string> { "F1", "F2", "F3", "F4", "E1", "E2" };
            Binding b = new Binding();
            b.Source = mdv;
            b.Path = new PropertyPath("NumbersFormat");
            comboBox_format.SetBinding(ComboBox.SelectedValueProperty, b);
        }

        private void ChartInit()
        {
            winFormsHost.Child = chart;
            chart.ChartAreas.Add(new ChartArea("area1"));
            chart.ChartAreas.Add(new ChartArea("area2"));
            chart.BackColor = Color.LightGray;

            chart.ChartAreas[0].AxisX.Title = "x";
            chart.ChartAreas[0].AxisX.LabelStyle.Format = mdv.NumbersFormat;
            chart.ChartAreas[0].AxisX.IsMarginVisible = false;
            chart.ChartAreas[0].AxisY.Title = "f(x; P)";
            chart.ChartAreas[0].AxisY.LabelStyle.Format = mdv.NumbersFormat;

            chart.ChartAreas[1].AxisX.Title = "p";
            chart.ChartAreas[1].AxisX.LabelStyle.Format = mdv.NumbersFormat;
            chart.ChartAreas[1].AxisX.IsMarginVisible = false;
            chart.ChartAreas[1].AxisY.Title = "f(p; X)";
            chart.ChartAreas[1].AxisY.LabelStyle.Format = mdv.NumbersFormat;

            chart.Legends.Add(new Legend("Legend1"));
            chart.Legends["Legend1"].DockedToChartArea = "area1";
            chart.Legends.Add(new Legend("Legend2"));
            chart.Legends["Legend2"].DockedToChartArea = "area2";
        }

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                DataContext = myModelDataCollection;
                BindCustomModelData();
                BindModelDataView();
                BindFormat();
                ChartInit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NewCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (myModelDataCollection.ChangesUnsaved)
            {
                MessageBoxResult result = MessageBox.Show("Save changes?", "", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    if ((bool)saveDialog.ShowDialog())
                    {
                        bool res = ObservableModelData.Save(saveDialog.FileName, myModelDataCollection);
                        if (!res)
                        {
                            MessageBox.Show("Error saving file");
                            return;
                        }
                    }
                }
                else if (result == MessageBoxResult.Cancel) return;
            }
            myModelDataCollection.Clear();
            myModelDataCollection.ChangesUnsaved = false;
        }

        private void OpenCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if ((bool)openDialog.ShowDialog())
            {
                if (myModelDataCollection.ChangesUnsaved)
                {
                    MessageBoxResult result = MessageBox.Show("Save changes?", "", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Yes)
                    {
                        SaveFileDialog saveDialog = new SaveFileDialog();
                        if ((bool)saveDialog.ShowDialog())
                        {
                            bool res = ObservableModelData.Save(saveDialog.FileName, myModelDataCollection);
                            if (!res)
                            {
                                MessageBox.Show("Error saving file");
                                return;
                            }
                        }

                    }
                    else if (result == MessageBoxResult.Cancel) return;
                }

                if (ObservableModelData.Load(openDialog.FileName, ref myModelDataCollection))
                {
                    myModelDataCollection.ChangesUnsaved = false;
                    DataContext = myModelDataCollection;
                    myModelDataCollection.CollectionChanged += myModelDataCollection.updateFlag;
                }
                else
                {
                    MessageBox.Show("Error opening file");
                }
            }
        }

        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            if ((bool)saveDialog.ShowDialog())
            {
                bool res = ObservableModelData.Save(saveDialog.FileName, myModelDataCollection);
                if (!res)
                {
                    MessageBox.Show("Error saving file");
                    return;
                }
            }
        }

        private void CanSaveCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = myModelDataCollection.ChangesUnsaved;
        }

        private void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            while (listBox.SelectedItems.Count > 0)
            {
                int selected = listBox.SelectedIndex;
                myModelDataCollection.Remove_At(selected);
            }
        }

        private void CanDeleteCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = listBox.SelectedItems.Count > 0;
        }

        private void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            myModelDataCollection.Add(customMd);
            customMd = new ModelData();
            BindCustomModelData();
        }

        private void CanAddCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !(Validation.GetHasError(textBox_parameter) || Validation.GetHasError(textBox_nodes));
        }
    
        private void DrawCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                List<ModelData> selected = new List<ModelData>();
                foreach (var m in listBox.SelectedItems) selected.Add(m as ModelData);
                mdv.Draw(chart, selected);
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CanDrawCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (listBox.SelectedItems.Count > 0) && (!Validation.GetHasError(textBox_xValue));
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (myModelDataCollection.ChangesUnsaved)
            {
                MessageBoxResult result = MessageBox.Show("Save changes?", "", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    if ((bool)saveDialog.ShowDialog())
                    {
                        bool res = ObservableModelData.Save(saveDialog.FileName, myModelDataCollection);
                        if (!res)
                        {
                            MessageBox.Show("Error saving file");
                            return;
                        }
                    }
                    else e.Cancel = true;
                }
                else if (result == MessageBoxResult.Cancel) e.Cancel = true;
            }
        }

    }
}
