using System.Drawing;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Win32;
using ViewModel;

namespace Lab3
{
    public class MyAppUIServices : IUIServices
    {
        public bool ConfirmDelete()
        {
            return MessageBox.Show("Delete?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        public bool ConfirmSave()
        {
            return MessageBox.Show("Save changes?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        public string SaveDialog()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.ShowDialog();
            return saveDialog.FileName;
        }

        public string OpenDialog()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.ShowDialog();
            return openDialog.FileName;
        }

        public ListBox listBox;
        public MeshFunctionCollection mfc;
        public MeshFunctionVisible mfv;
        public MeshFunction customMF;
        public TextBox textBox_parameter;
        public TextBox textBox_nodes;
        public TextBox textBox_xValue;
        public Chart chart;

        public void UpdateBinding()
        {
            Binding b = new Binding();
            b.Path = new PropertyPath("Collection");
            b.Source = mfc;
            listBox.SetBinding(ListBox.ItemsSourceProperty, b);
        }

        public void AddElement()
        {
            mfc.Add(customMF);
        }

        public bool NoErrorsAdd()
        {
            return !(Validation.GetHasError(textBox_parameter) || Validation.GetHasError(textBox_nodes));
        }

        public bool NoErrorsDraw()
        {
            return !Validation.GetHasError(textBox_xValue);
        }

        public void DrawElement()
        {
            mfv.Draw(chart, mfc, listBox.SelectedItems);
        }
    }

    public partial class MainWindow : Window
    {
        private MeshFunctionVisible mfv;
        private static MyAppUIServices uiServices = new MyAppUIServices();
        private MeshFunctionCollection mfc = new MeshFunctionCollection(uiServices);
        private MeshFunction customMF;
        private Chart chart = new Chart(); 

        private void ChartInit()
        {
            winFormsHost.Child = chart;
            chart.ChartAreas.Add(new ChartArea("area1"));
            chart.ChartAreas.Add(new ChartArea("area2"));
            chart.BackColor = Color.LightGray;

            chart.ChartAreas[0].AxisX.Title = "x";
            chart.ChartAreas[0].AxisX.LabelStyle.Format = mfv.NumberFormat;
            chart.ChartAreas[0].AxisX.IsMarginVisible = false;
            chart.ChartAreas[0].AxisY.Title = "f(x; P)";
            chart.ChartAreas[0].AxisY.LabelStyle.Format = mfv.NumberFormat;

            chart.ChartAreas[1].AxisX.Title = "p";
            chart.ChartAreas[1].AxisX.LabelStyle.Format = mfv.NumberFormat;
            chart.ChartAreas[1].AxisX.IsMarginVisible = false;
            chart.ChartAreas[1].AxisY.Title = "f(p; X)";
            chart.ChartAreas[1].AxisY.LabelStyle.Format = mfv.NumberFormat;

            chart.Legends.Add(new Legend("Legend1"));
            chart.Legends["Legend1"].DockedToChartArea = "area1";
            chart.Legends.Add(new Legend("Legend2"));
            chart.Legends["Legend2"].DockedToChartArea = "area2";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = mfc;
            mfv = TryFindResource("key_mfv") as MeshFunctionVisible;
            customMF = TryFindResource("key_customMF") as MeshFunction;

            uiServices.listBox = listBox;
            uiServices.mfc = mfc;
            uiServices.mfv = mfv;
            uiServices.customMF = customMF;
            uiServices.textBox_parameter = textBox_parameter;
            uiServices.textBox_nodes = textBox_nodes;
            uiServices.textBox_xValue = textBox_xValue;
            uiServices.chart = chart;
            ChartInit();
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            mfc.NewCommand.Execute(this);
        }
    }
}
