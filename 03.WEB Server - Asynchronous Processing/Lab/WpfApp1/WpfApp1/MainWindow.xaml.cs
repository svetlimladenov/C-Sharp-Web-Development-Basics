using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
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

namespace WpfApp1
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
            await DownloadAndShowImageAsync(Image1, "https://dynaimage.cdn.cnn.com/cnn/w_768,h_1024,c_scale/https%3A%2F%2Fdynaimage.cdn.cnn.com%2Fcnn%2Fx_1085%2Cy_0%2Cw_2578%2Ch_3437%2Cc_crop%2Fhttps%253A%252F%252Fstamp.static.cnn.io%252F5b7ac48b4db3d70020c01c13%252Fshutterstock_757946224.jpg");
            await DownloadAndShowImageAsync(Image2, "https://static.boredpanda.com/blog/wp-content/uploads/2017/09/funny-dog-thoughts-tweets-1.jpg");
            await DownloadAndShowImageAsync(Image3, "https://www.what-dog.net/Images/faces2/scroll001.jpg");
            await DownloadAndShowImageAsync(Image4, "https://www.nationalgeographic.com/content/dam/animals/thumbs/rights-exempt/mammals/d/domestic-dog_thumb.ngsversion.1484159404151.adapt.1900.1.jpg");
            await DownloadAndShowImageAsync(Image5, "https://i.ytimg.com/vi/C_lpU5DiJ0Y/maxresdefault.jpg");
            await DownloadAndShowImageAsync(Image6, "https://ksrpetcare.com/wp-content/uploads/2017/04/41059-Cute-Yellow-Labrador-puppy-in-play-bow-white-background.jpg");

        }

        public async Task DownloadAndShowImageAsync(Image image, string url)
        {
            WebClient webClient = new WebClient();
            var byteArray = await webClient.DownloadDataTaskAsync(url);
            await Task.Run(() => Thread.Sleep(1000));
            image.Source = LoadImage(byteArray);    
        }
        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
