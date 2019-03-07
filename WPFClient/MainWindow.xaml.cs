using DownloadLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.progressBar.Visibility = Visibility.Visible;
            var downloader = new SimpleDownloader();
            var path = await downloader.DownloadFile(
                ConfigurationManager.AppSettings["uri"],
                "installerFile.msi",
                ConfigurationManager.AppSettings["username"], 
                ConfigurationManager.AppSettings["password"]);

            this.progressBar.Visibility = Visibility.Collapsed;
            txtLog.AppendText($"Done.\n");
            txtLog.AppendText($"File path is {path}.\n");

            txtLog.AppendText($"Start process {path}.\n");
            Process.Start(path);
        }
    }
}
