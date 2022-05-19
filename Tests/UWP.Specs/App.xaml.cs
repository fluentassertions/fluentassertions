using System;
using Microsoft.VisualStudio.TestPlatform.TestExecutor;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UWP.Specs;

internal sealed partial class App : Application
{
    public App()
    {
        InitializeComponent();
        Suspending += OnSuspending;
    }

    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {
        if (Window.Current.Content is not Frame rootFrame)
        {
            rootFrame = new Frame();
            rootFrame.NavigationFailed += OnNavigationFailed;
            Window.Current.Content = rootFrame;
        }

        UnitTestClient.Run(e.Arguments);
    }

    private void OnNavigationFailed(object sender, NavigationFailedEventArgs e) =>
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);

    private void OnSuspending(object sender, SuspendingEventArgs e) =>
        e.SuspendingOperation.GetDeferral().Complete();
}
