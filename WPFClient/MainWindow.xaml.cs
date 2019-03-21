using DownloadLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtLog.AppendText($"Ask user to download file.\n");
            var result = MessageBox.Show("We need to download file.", "Download", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                txtLog.AppendText($"Start downloading.\n");
                this.progressBar.Visibility = Visibility.Visible;
                var downloader = new SimpleDownloader();
                var stream = await downloader.RequestHttpContentAsync(
                    ConfigurationManager.AppSettings["uri"],
                    ConfigurationManager.AppSettings["username"],
                    ConfigurationManager.AppSettings["password"]);

                this.progressBar.Visibility = Visibility.Collapsed;
                txtLog.AppendText($"Done.\n");

                var path = SaveFile(stream);
                txtLog.AppendText($"File path is {path}.\n");

                Process.Start(path);
                txtLog.AppendText($"Start process {path}.\n");
            }
        }

        private string SaveFile(Stream stream)
        {
            var filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "installFile.msi");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                byte[] buffer = new byte[2048];
                int bytesRead;
                do
                {
                    bytesRead = stream.Read(buffer, 0, 2048);
                    fileStream.Write(buffer, 0, bytesRead);
                } while (bytesRead > 0);

            }

            return filePath;
        }
    }
}
