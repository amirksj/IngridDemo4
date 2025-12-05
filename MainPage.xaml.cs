using Camera.MAUI;
using Plugin.Maui.OCR;

namespace IngridDemo4
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            OcrPlugin.Default.InitAsync();
        }

        private void cameraView_CamerasLoaded(object sender, EventArgs e)
        {
            cameraView.Camera = cameraView.Cameras.First();

            MainThread.BeginInvokeOnMainThread(async () =>
            {

                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });
        }

        /*private void Button_Clicked(object sender, EventArgs e)
        {
            myImage.Source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
        }*/

        private async void CounterBtn_Clicked(object sender, EventArgs e)
        {
            try { 
            var pickResult = await MediaPicker.Default.PickPhotoAsync();
                if (pickResult != null)
                {
                    using var imageAsStream = await pickResult.OpenReadAsync();
                    var imageAsBytes = new byte[imageAsStream.Length];
                    await imageAsStream.ReadAsync(imageAsBytes);

                    var ocrResult = await OcrPlugin.Default.RecognizeTextAsync(imageAsBytes);

                    if (!ocrResult.Success) 
                    {
                        await DisplayAlert("No Success", "No OCR possible", "OK");
                        return;
                    }

                    await DisplayAlert("OCR Result", ocrResult.AllText, "OK");

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void PictureBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                var pickResult = await MediaPicker.Default.CapturePhotoAsync();
                if (pickResult != null)
                {
                    using var imageAsStream = await pickResult.OpenReadAsync();
                    var imageAsBytes = new byte[imageAsStream.Length];
                    await imageAsStream.ReadAsync(imageAsBytes);

                    var ocrResult = await OcrPlugin.Default.RecognizeTextAsync(imageAsBytes);

                    if (!ocrResult.Success)
                    {
                        await DisplayAlert("No Success", "No OCR possible", "OK");
                        return;
                    }

                    await DisplayAlert("OCR Result", ocrResult.AllText, "OK");

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
