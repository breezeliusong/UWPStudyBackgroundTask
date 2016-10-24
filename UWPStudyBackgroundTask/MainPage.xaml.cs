using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPStudyBackgroundTask.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPStudyBackgroundTask
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

        private async void Launch(object sender, RoutedEventArgs e)
        {
            var testAppUri = new Uri("myapp:"); // The protocol handled by the launched app
            var options = new LauncherOptions();
            //options.TargetApplicationPackageFamilyName = "bde3f60e-aa19-45a5-88c6-bc361b7eeca1_hr5wmtferjq42";

            var success = await Windows.System.Launcher.LaunchUriAsync(testAppUri);
            //var inputData = new ValueSet();
            //inputData["TestData"] = "Test data";

            //string theResult = "";
            //LaunchUriResult result = await Windows.System.Launcher.LaunchUriForResultsAsync(testAppUri, options, inputData);
            //if (result.Status == LaunchUriStatus.Success &&
            //    result.Result != null &&
            //    result.Result.ContainsKey("ReturnedData"))
            //{
            //    ValueSet theValues = result.Result;
            //    theResult = theValues["ReturnedData"] as string;
            //    Debug.WriteLine(theResult);
            //}
        }
    }
}
