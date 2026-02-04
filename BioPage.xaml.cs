namespace MauiProject;

public partial class BioPage : ContentPage
{
    private string _selectedGender = "Male"; // Default
    private string _activityLevel = "Low";
    
    // Risk factors state
    private bool _hasAlcohol = false;
    private bool _hasSmoking = false;
    private bool _hasStress = false;
    private bool _hasSleepIssues = false;

    public BioPage()
    {
        InitializeComponent();
        UpdateGenderUI();
    }

    private void OnMaleSelected(object sender, TappedEventArgs e)
    {
        _selectedGender = "Male";
        UpdateGenderUI();
    }

    private void OnFemaleSelected(object sender, TappedEventArgs e)
    {
        _selectedGender = "Female";
        UpdateGenderUI();
    }

    private void UpdateGenderUI()
    {
        bool isMale = _selectedGender == "Male";
        
        MaleCard.Stroke = isMale ? Color.FromArgb("#4CAF50") : Color.FromArgb("#333333");
        MaleCard.BackgroundColor = isMale ? Color.FromArgb("#2E2E2E") : Color.FromArgb("#1E1E1E");
        
        FemaleCard.Stroke = !isMale ? Color.FromArgb("#4CAF50") : Color.FromArgb("#333333");
        FemaleCard.BackgroundColor = !isMale ? Color.FromArgb("#2E2E2E") : Color.FromArgb("#1E1E1E");
    }

    private void OnActivityChanged(object sender, ValueChangedEventArgs e)
    {
        int step = (int)Math.Round(e.NewValue);
        switch (step)
        {
            case 0:
                ActivityLabel.Text = "Офис (Сидячий)";
                _activityLevel = "Low";
                break;
            case 1:
                ActivityLabel.Text = "Зал 2-3 раза (Средняя)";
                _activityLevel = "Medium";
                break;
            case 2:
                ActivityLabel.Text = "Атлет (Высокая)";
                _activityLevel = "High";
                break;
        }
    }

    private void OnRiskToggled(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            // Determine which risk factor
            bool isSelected = false;

            if (btn == RiskAlcohol) { _hasAlcohol = !_hasAlcohol; isSelected = _hasAlcohol; }
            else if (btn == RiskSmoking) { _hasSmoking = !_hasSmoking; isSelected = _hasSmoking; }
            else if (btn == RiskStress) { _hasStress = !_hasStress; isSelected = _hasStress; }
            else if (btn == RiskSleep) { _hasSleepIssues = !_hasSleepIssues; isSelected = _hasSleepIssues; }

            // Update UI for this button
            if (isSelected)
            {
                btn.BackgroundColor = Color.FromArgb("#2E2E2E");
                btn.TextColor = Colors.White;
                btn.BorderColor = Color.FromArgb("#6200EE");
            }
            else
            {
                btn.BackgroundColor = Color.FromArgb("#1E1E1E");
                btn.TextColor = Colors.Gray;
                btn.BorderColor = Color.FromArgb("#333333");
            }
        }
    }

    private void OnFinishClicked(object sender, EventArgs e)
    {
        // Save Preferences
        Preferences.Set("Gender", _selectedGender);
        Preferences.Set("ActivityLevel", _activityLevel);
        
        var badHabits = new List<string>();
        if (_hasAlcohol) badHabits.Add("Alcohol");
        if (_hasSmoking) badHabits.Add("Smoking");
        if (_hasStress) badHabits.Add("Stress");
        if (_hasSleepIssues) badHabits.Add("SleepIssues");
        
        // Save complex object (simplified as comma separated string or individual bools for now)
        // Ideally serialize to UserProfile, but user asked for Preferences.Set logic.
        Preferences.Set("Risk_Alcohol", _hasAlcohol);
        Preferences.Set("Risk_Smoking", _hasSmoking);
        Preferences.Set("Risk_Stress", _hasStress);
        Preferences.Set("Risk_Sleep", _hasSleepIssues);

        // Mark onboarding as complete (optional but good practice)
        Preferences.Set("OnboardingFinished", true);

        // RESET NAVIGATION TO MAIN APP
        Application.Current.MainPage = new AppShell();
    }
}
