using Microsoft.VisualStudio.TestPlatform.TestExecutor;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace UWP.Specs;

internal sealed partial class App : Application
{
    public App()
    {
        InitializeComponent();
        Suspending += OnSuspending;
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        UnitTestClient.Run(args.Arguments);
    }

    private void OnSuspending(object sender, SuspendingEventArgs e) =>
        e.SuspendingOperation.GetDeferral().Complete();
}
