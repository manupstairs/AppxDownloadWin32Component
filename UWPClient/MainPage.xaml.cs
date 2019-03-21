using DownloadLib;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            txtLog.Text += $"Ask user to download file.\n";
            var dialog = new MessageDialog("Do you want to download installation file?");
            dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
            var res = await dialog.ShowAsync();

            if ((int)res.Id == 0)
            {
                txtLog.Text += $"Start downloading.\n";
                this.progressRing.IsActive = true;
                var downloader = new SimpleDownloader();
                var stream = await downloader.RequestHttpContentAsync(
                    "",
                    "",
                    "");

                this.progressRing.IsActive = false;
                var file = await SaveStorageFile(stream);
                var result = await Launcher.LaunchFolderAsync(ApplicationData.Current.LocalFolder);
                txtLog.Text += $"Done.\n ";
            }
        }

        private async Task<IStorageFile> SaveStorageFile(Stream stream)
        {
            var installFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("installerFile.msi", CreationCollisionOption.ReplaceExisting);
            using (var openStream = await installFile.OpenStreamForWriteAsync())
            {
                var buffer = new byte[2048];
                int bytesRead;
                do
                {
                    bytesRead = stream.Read(buffer, 0, 2048);
                    await openStream.WriteAsync(buffer, 0, bytesRead);
                }
                while (bytesRead > 0);
            }

            return installFile;
        }
    }
}
