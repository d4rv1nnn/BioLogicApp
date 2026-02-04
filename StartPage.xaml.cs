using MauiProject; // For MainPage

namespace BioLogicNative;

public partial class StartPage : ContentPage
{
	public StartPage()
	{
		InitializeComponent();
	}

    private async void OnStartClicked(object sender, EventArgs e)
    {
        // Переход на главную (MainPage)
        await Shell.Current.GoToAsync(nameof(MainPage));
    }
}
