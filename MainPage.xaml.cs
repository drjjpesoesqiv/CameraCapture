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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;

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
			// create new camera capture object and set options
			CameraCaptureUI cameraCapture = new CameraCaptureUI();
			cameraCapture.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
			cameraCapture.PhotoSettings.AllowCropping = false;

			// call capture file sync and specify photo should be captured
			StorageFile photo = await cameraCapture.CaptureFileAsync(CameraCaptureUIMode.Photo);

			// exit if photo is null, ie cancelled operation
			if (photo == null)
				return;

			// get reference to ~/Pictures folder
			var pictures = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures);
			StorageFolder picturesFolder = pictures.SaveFolder;

			// copy photo to pictures directory
			await photo.CopyAsync(picturesFolder, "ProfilePhoto.jpg", NameCollisionOption.ReplaceExisting);
			
			// stream from file and decode bitmap
			IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
			BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
			SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();

			// convert image to be readable by image control, BGRA8 format
			SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(
				softwareBitmap,
				BitmapPixelFormat.Bgra8,
				BitmapAlphaMode.Premultiplied);

			// assign software bitmap to bitmap source
			SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
			await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

			// set bitmap source as image control source
			ImageControl.Source = bitmapSource;

			// delete storage file object
			await photo.DeleteAsync();
		}
	}
}
