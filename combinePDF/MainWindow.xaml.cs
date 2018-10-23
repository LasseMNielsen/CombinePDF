using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using combinePDF.Functions;
using combinePDF.Objects;
using Microsoft.Win32;

namespace combinePDF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string outputPath = "";
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Events
            buttonSaveAs.Click += ButtonSaveAsOnClick;
            buttonInputFiles.Click += ButtonInputFilesOnClick;
            buttonRun.Click += ButtonRunOnClick;
            menuAbout.Click += MenuAboutOnClick;

            listboxPdfFiles.Drop += ListboxPdfFilesOnDrop;
        }

        private void MenuAboutOnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Writte by Lasse M.N\n2018");
        }

        // drag and drop
        private void ListboxPdfFilesOnDrop(object sender, DragEventArgs e)
        {
            // get files
            var dragFiles = (string[])e.Data.GetData(DataFormats.FileDrop, true);

            listboxPdfFiles.ItemsSource = listboxPdfObjects.createList(dragFiles.ToList());
            labelFileCount.Content = $"PDF filer: {listboxPdfFiles.Items.Count}";
        }

        // Combine files
        private void ButtonRunOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(outputPath))
            {
                ButtonSaveAsOnClick(null, null);
            }

            if (!string.IsNullOrEmpty(outputPath))
            {
                if (listboxPdfFiles.Items.Count < 1)
                {
                    ButtonInputFilesOnClick(null, null);
                }

                var files = listboxPdfFiles.Items.OfType<listboxPdfObjects>().ToList();

                if (files.Count > 0 && !string.IsNullOrEmpty(outputPath))
                {
                    PDF.combinePDF(outputPath, files);
                    listboxPdfFiles.ItemsSource = null;
                    labelFileCount.Content = "PDF filer: 0";
                    MessageBox.Show($"Pdfs combined @ {outputPath}","Success",MessageBoxButton.OK,MessageBoxImage.Information);
                }
            }
        }

        // Select files
        private void ButtonInputFilesOnClick(object sender, RoutedEventArgs e)
        {
            List<string> listFiles = new List<string>();
            List<listboxPdfObjects> listBoxItems = new List<listboxPdfObjects>();

            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            dialog.Filter = "PDF Files (*.pdf)|*.pdf";
            dialog.DefaultExt = "pdf";
            dialog.Multiselect = true;

            var dialogResult = dialog.ShowDialog();

            if (dialogResult.Value)
            {
                listFiles = dialog.FileNames.ToList();
            }

            if (listFiles.Count > 0)
            {
                listBoxItems = listboxPdfObjects.createList(listFiles);
                listboxPdfFiles.ItemsSource = listBoxItems;
                labelFileCount.Content = $"PDF filer: {listBoxItems.Count}";
            }
        }

        // select outputs
        private void ButtonSaveAsOnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();

            if (listboxPdfFiles.Items.Count > 0)
            {
                var item = listboxPdfFiles.Items[0] as listboxPdfObjects;
                dialog.InitialDirectory = Directory.GetParent(item.fullname).ToString();
            }
            else
            {
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
                
            dialog.Filter = "PDF Files (*.pdf)|*.pdf";
            dialog.DefaultExt = "pdf";
            dialog.AddExtension = true;

            var dialogResult = dialog.ShowDialog();

            if (dialogResult.Value)
            {
                outputPath = dialog.FileName;
                labelOutputPath.Content = dialog.FileName;
            }
        }
    }
}
