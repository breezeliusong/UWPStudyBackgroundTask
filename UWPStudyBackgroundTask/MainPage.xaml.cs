using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPStudyBackgroundTask.Pages;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
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


        private async void RegisterBackgroundTask()
        {
            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatus == BackgroundAccessStatus.AlwaysAllowed ||
                backgroundAccessStatus == BackgroundAccessStatus.AllowedSubjectToSystemPolicy)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                    {
                        task.Value.Unregister(true);
                    }
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = taskName;
                taskBuilder.TaskEntryPoint = taskEntryPoint;
                taskBuilder.SetTrigger(new TimeTrigger(15, false));
                var registration = taskBuilder.Register();
            }
        }

        private const string taskName = "BlogFeedBackgroundTask";
        private const string taskEntryPoint = "BackgroundTasks.BlogFeedBackgroundTask";
    


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

        private async void InProcessBackground(object sender, RoutedEventArgs e)
        {
            var builder = new BackgroundTaskBuilder();
            builder.Name = "My Background Trigger";
            builder.SetTrigger(new SystemTrigger(SystemTriggerType.TimeZoneChange, false));
            BackgroundTaskRegistration task = builder.Register();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() => bt.Content = "great"));

        }

        private async void OnProgress(IBackgroundTaskRegistration task, BackgroundTaskProgressEventArgs args)
        {
            var progress = "progress:" + args.Progress + "%";
            Debug.WriteLine(progress);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
             () =>
            {
                MyTextBlock.Text = progress;
            });

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.RegisterBackgroundTask();
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == "ExampleBackgroundTask")
                {
                    AttachProgressAndCompletedHandlers(task.Value);
                }
            }

        }


        private void AttachProgressAndCompletedHandlers(IBackgroundTaskRegistration task)
        {
            task.Progress += new BackgroundTaskProgressEventHandler(OnProgress);
            task.Completed += new BackgroundTaskCompletedEventHandler(OnCompleted);
        }

        private async void OnCompleted(IBackgroundTaskRegistration task, BackgroundTaskCompletedEventArgs args)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var key = task.TaskId.ToString();
            var message = settings.Values.ToString();
            Debug.WriteLine(message);
            Debug.WriteLine("nihao");
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            async () =>
            {
                await new MessageDialog("succees").ShowAsync();
            });
        }

        bool taskRegistered = false;
        BackgroundTaskBuilder builder;
        BackgroundTaskRegistration taskBack;
        private async void OutProcessBackground(object sender, RoutedEventArgs e)
        {
            //Background
            var exampleTaskName = "ExampleBackgroundTask";

            foreach (var tas in BackgroundTaskRegistration.AllTasks)
            {
                if (tas.Value.Name == exampleTaskName)
                {
                    taskRegistered = true;
                    taskBack = (BackgroundTaskRegistration)tas.Value;
                    break;
                }
            }
             builder = new BackgroundTaskBuilder();

            //Universal Windows apps must call RequestAccessAsync before registering any of the background trigger types.
            builder.Name = exampleTaskName;
            builder.TaskEntryPoint = "Background.Tasks";

            await BackgroundExecutionManager.RequestAccessAsync();
            builder.SetTrigger(new SystemTrigger(SystemTriggerType.TimeZoneChange, false));
            //builder.AddCondition(new SystemCondition(SystemConditionType.UserPresent));
            if (!taskRegistered)
            {
                taskBack = builder.Register();
                
            }
                taskBack.Completed += new BackgroundTaskCompletedEventHandler(OnCompleted);
        }

        //how to manually cancel a background,trigger changed then the background is cancelled;timertrigger
        private void Cancell(object sender, RoutedEventArgs e)
        {
            taskBack.Unregister(true);

            //builder.CancelOnConditionLoss=true;
        }

        private void Tile(object sender, RoutedEventArgs e)
        {

        }
    }
}
