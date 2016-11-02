using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using Windows.UI.Xaml.Controls;

namespace Background
{
    public sealed class Tasks : IBackgroundTask
    {
        // Note: defined at class scope so we can mark it complete inside the OnCancel() callback if we choose to support cancellation
        BackgroundTaskDeferral _deferral;
        volatile bool _CancelRequested = false;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            // TODO: Insert code to start one or more asynchronous methods using the
            //       await keyword, for example:
            //
            // await ExampleMethodAsync();
            Debug.WriteLine("hello");
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCancelled);
            _deferral.Complete();
        }


        public void OnCancelled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            // TODO: Add code to notify the background task that it is cancelled.
            _CancelRequested = true;
            Debug.WriteLine("background" + sender.Task.Name + "cancell Requested");
        }

        public static BackgroundTaskRegistration RegisterBackgroundTask(
                                                string taskEntryPoint,
                                                string name,
                                                IBackgroundTrigger trigger,
                                                IBackgroundCondition condition)
        {
            //
            // Check for existing registrations of this background task.
            //

            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {

                if (cur.Value.Name == name)
                {
                    //
                    // The task is already registered.
                    //

                    return (BackgroundTaskRegistration)(cur.Value);
                }
            }


            // We'll register the task in the next step.
            //
            // Register the background task.
            //

            var builder = new BackgroundTaskBuilder();

            builder.Name = name;

            // in-process background tasks don't set TaskEntryPoint
            if (taskEntryPoint != null && taskEntryPoint != String.Empty)
            {
                builder.TaskEntryPoint = taskEntryPoint;
            }
            builder.SetTrigger(trigger);

            if (condition != null)
            {
                builder.AddCondition(condition);
            }

            BackgroundTaskRegistration task = builder.Register();

            return task;

        }

        //
        // Register a background task with the specified taskEntryPoint, name, trigger,
        // and condition (optional).
        //
        // taskEntryPoint: Task entry point for the background task.
        // taskName: A name for the background task.
        // trigger: The trigger for the background task.
        // condition: Optional parameter. A conditional event that must be true for the task to fire.
        //
        public static BackgroundTaskRegistration RegisterBackgroundTasks(string taskEntryPoint,
                                                                        string taskName,
                                                                        IBackgroundTrigger trigger,
                                                                        IBackgroundCondition condition)
        {
            //
            // Check for existing registrations of this background task.
            //

            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {

                if (cur.Value.Name == taskName)
                {
                    //
                    // The task is already registered.
                    //

                    return (BackgroundTaskRegistration)(cur.Value);
                }
            }

            //
            // Register the background task.
            //

            var builder = new BackgroundTaskBuilder();

            builder.Name = taskName;
            builder.TaskEntryPoint = taskEntryPoint;
            builder.SetTrigger(trigger);

            if (condition != null)
            {

                builder.AddCondition(condition);
            }

            BackgroundTaskRegistration task = builder.Register();

            return task;
        }
    }
}
