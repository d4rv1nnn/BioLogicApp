namespace MauiProject;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(BioLogicNative.ResultPage), typeof(BioLogicNative.ResultPage));
        Routing.RegisterRoute(nameof(BioLogicNative.SummaryPage), typeof(BioLogicNative.SummaryPage));
        Routing.RegisterRoute(nameof(BioLogicNative.StartPage), typeof(BioLogicNative.StartPage));
        Routing.RegisterRoute(nameof(MauiProject.MainPage), typeof(MauiProject.MainPage));
	}
}
