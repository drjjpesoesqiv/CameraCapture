using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CameraCapture
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

		private async void CameraCaptureBtn_Click(object sender, RoutedEventArgs e)
		{
			CameraCaptureUI cameraCapture = new CameraCaptureUI();
			cameraCapture.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
			cameraCapture.PhotoSettings.CroppedSizeInPixels = new Size(512, 512);

			StorageFile photo = await cameraCapture.CaptureFileAsync(CameraCaptureUIMode.Photo);

			if (photo == null)
				return;

			var pictures = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures);
			StorageFolder picturesFolder = pictures.SaveFolder;

			await photo.CopyAsync(picturesFolder, "ProfilePhoto.jpg", NameCollisionOption.ReplaceExisting);
			await photo.DeleteAsync();
		}
	}
}
