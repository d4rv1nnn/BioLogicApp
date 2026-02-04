namespace MauiProject;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Проверяем, проходил ли пользователь регистрацию
        bool hasProfile = Preferences.ContainsKey("UserName");
        if (hasProfile)
        {
            MainPage = new AppShell(); // Сразу в приложение
        }
        else
        {
            // Если первый раз — показываем регистрацию (без Shell, просто страница)
            MainPage = new NavigationPage(new WelcomePage()); 
        }
    }
}