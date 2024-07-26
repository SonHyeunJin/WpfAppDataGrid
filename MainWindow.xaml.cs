using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace WpfAppDataGrid
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 파일 열기 버튼 클릭 이벤트 핸들러
        private void FileOpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Open CSV File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    FilePathLabel.Content = $"File Path: {filePath}";
                    List<Data> dataList = ParseCsv(filePath);
                    DataGrid.ItemsSource = dataList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // CSV 파일 파싱
        private List<Data> ParseCsv(string filePath)
        {
            var dataList = new List<Data>();
            var encoding = DetectEncoding(filePath);

            using (var reader = new StreamReader(filePath, encoding))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(',');

                    if (values.Length >= 3)
                    {
                        dataList.Add(new Data
                        {
                            Name = values[0].Trim(),
                            Age = values[1].Trim(),
                            Explain = values[2].Trim()
                        });
                    }
                }
            }

            return dataList;
        }

        // 파일 인코딩 탐지
        private Encoding DetectEncoding(string filePath)
        {
            // 기본적으로 UTF-8 인코딩을 사용
            return Encoding.UTF8;
        }

        // 데이터 가져오기 버튼 클릭 이벤트 핸들러
        private void GetDataButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = DataGrid.SelectedItems;
            if (selectedItems.Count > 0)
            {
                StringBuilder message = new StringBuilder("Selected Names:\n");
                foreach (var item in selectedItems)
                {
                    if (item is Data selectedData)
                    {
                        message.AppendLine(selectedData.Name);
                    }
                }
                MessageBox.Show(message.ToString(), "Selected Data", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No items selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // 데이터 클래스 정의
        public class Data
        {
            public string Name { get; set; }
            public string Age { get; set; }
            public string Explain { get; set; }
        }
    }
}
