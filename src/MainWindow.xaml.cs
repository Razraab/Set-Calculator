using Microsoft.Win32;
using NumericSetCalculator;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SetCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NumericSet numericSet = new NumericSet();
        private int count = 0;

        public MainWindow()
        {
            InitializeComponent();            
        }

        private void UpdateProperties()
        {
            SumControl.Text = $"Sum: {numericSet.GetSum()}";
            ArifmeticMeanControl.Text = $"Ar. mean: {numericSet.GetArifmeticMean()}";
            MedianControl.Text = $"Median: {numericSet.GetMedian()}";
            SpanControl.Text = $"Scope: {numericSet.GetSpan()}";
            DispersionControl.Text = $"Dispersion: {numericSet.GetDispersion()}";
        }

        private void AddNumber(object sender, RoutedEventArgs e)
        {
            numericSet.Add(0);
            TextBox textBox = new TextBox
            {
                Text = "0",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 21,
                MinWidth = 150,
                MinHeight = 40
            };
            textBox.TextChanged += OnValueChanged;
            textBox.PreviewTextInput += OnPreviewTextInput;
            textBox.TabIndex = count;
            count++;
            NumericSetControl.Items.Add(textBox);
            UpdateProperties();
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(IsDigit);
        }

        bool IsDigit(char c)
        {
            if (c >= '0' && c <= '9')
                return true;
            return false;
        }

        private void OnValueChanged(object sender, TextChangedEventArgs e)
        {
            if(e.Source is TextBox textBox && textBox.Text != "")
            {
                double number = double.Parse(textBox.Text);
                int index = textBox.TabIndex;
                numericSet.Update(number, index);
            }
            UpdateProperties();
        }

        private void RemoveNumber(object sender, RoutedEventArgs e)
        {
            try
            {
                numericSet.Remove(double.Parse(((TextBox)NumericSetControl.SelectedItem).Text));
                NumericSetControl.Items.Remove(NumericSetControl.SelectedItem);
                UpdateProperties();
            }
            catch
            {
                MessageBox.Show("No element is highlighted", "Warning", 
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            count = 0;
            numericSet.Clear();
            NumericSetControl.Items.Clear();
        }

        private void LoadFromFile(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.ShowDialog(this);

                string path = fileDialog.FileName;
                string innerText = File.ReadAllText(path);
                if (!path.EndsWith(".txt")) return;

                string[] values = innerText.Split(' ');
                List<double> numbers = new List<double>();
                foreach (string value in values)
                {
                    if (value == "") continue;
                    numbers.Add(double.Parse(value));
                }
                Clear(sender, e);
                foreach (double number in numbers)
                {
                    numericSet.Add(number);
                    TextBox textBox = new TextBox
                    {
                        Text = number.ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Top,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        FontSize = 21,
                        MinWidth = 150,
                        MinHeight = 40
                    };
                    textBox.TextChanged += OnValueChanged;
                    textBox.PreviewTextInput += OnPreviewTextInput;
                    textBox.TabIndex = count;
                    count++;
                    NumericSetControl.Items.Add(textBox);
                    UpdateProperties();
                }
            }
            catch
            {
                MessageBox.Show("The error is caused by an incorrect file extension or content!",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
