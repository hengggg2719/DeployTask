using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using MathLib;
using Newtonsoft.Json;

namespace CalApp
{
    public partial class MainWindow : Window
    {
        private readonly Calculator _calc = new Calculator();
        private readonly List<string> _history = new List<string>();
        private readonly string _historyPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CalApp", "history.json");

        public MainWindow()
        {
            InitializeComponent();
            Directory.CreateDirectory(Path.GetDirectoryName(_historyPath));
        }

        private (double a, double b) ReadInputs()
        {
            double.TryParse(TxtA.Text, out double a);
            double.TryParse(TxtB.Text, out double b);
            return (a, b);
        }

        private void ShowResult(double result, string op)
        {
            TxtResult.Text = $"Result: {result}";
            _history.Add($"{op} => {result}");
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var (a, b) = ReadInputs();
            ShowResult(_calc.Add(a, b), $"{a} + {b}");
        }

        private void Subtract_Click(object sender, RoutedEventArgs e)
        {
            var (a, b) = ReadInputs();
            ShowResult(_calc.Subtract(a, b), $"{a} - {b}");
        }

        private void Multiply_Click(object sender, RoutedEventArgs e)
        {
            var (a, b) = ReadInputs();
            ShowResult(_calc.Multiply(a, b), $"{a} * {b}");
        }

        private void Divide_Click(object sender, RoutedEventArgs e)
        {
            var (a, b) = ReadInputs();
            try
            {
                ShowResult(_calc.Divide(a, b), $"{a} / {b}");
            }
            catch (DivideByZeroException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveHistory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var json = JsonConvert.SerializeObject(_history, Formatting.Indented);
                File.WriteAllText(_historyPath, json);
                MessageBox.Show("History saved to " + _historyPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save history: " + ex.Message);
            }
        }

        private void LoadHistory_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(_historyPath))
            {
                MessageBox.Show("No history file found yet.");
                return;
            }
            var json = File.ReadAllText(_historyPath);
            var loaded = JsonConvert.DeserializeObject<List<string>>(json);
            LstHistory.ItemsSource = loaded;
        }
    }
}