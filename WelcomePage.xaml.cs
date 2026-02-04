namespace MauiProject;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        InitializeComponent();
    }

    private async void OnNextClicked(object sender, EventArgs e)
    {
        // Simple Validation
        if (string.IsNullOrWhiteSpace(NameEntry.Text) || 
            string.IsNullOrWhiteSpace(AgeEntry.Text) || 
            string.IsNullOrWhiteSpace(WeightEntry.Text))
        {
            await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "ОК");
            return;
        }

        // Save preliminary data
        Preferences.Set("UserName", NameEntry.Text);
        if (int.TryParse(AgeEntry.Text, out int age)) Preferences.Set("UserAge", age);
        if (double.TryParse(WeightEntry.Text, out double weight)) Preferences.Set("UserWeight", weight);

        // Navigate to BioPage
        await Navigation.PushAsync(new BioPage());
    }
}
