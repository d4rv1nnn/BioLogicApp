using BioLogicNative.Models;
using System.Collections.ObjectModel;

namespace BioLogicNative;

public partial class ResultPage : ContentPage
{
    public ObservableCollection<Hormone> DetectedHormones { get; set; } = new();

    public ResultPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public ResultPage(List<Hormone> problems) : this()
    {
        DetectedHormones.Clear();
        foreach (var p in problems)
        {
            DetectedHormones.Add(p);
        }
        ResultsList.ItemsSource = DetectedHormones;
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }

    private void OnItemTapped(object sender, TappedEventArgs e)
    {
        var hormone = e.Parameter as Hormone;
        if (hormone == null) return;

        hormone.IsExpanded = !hormone.IsExpanded;

        // Хак для обновления UI
        var currentList = ResultsList.ItemsSource;
        ResultsList.ItemsSource = null;
        ResultsList.ItemsSource = currentList;
    }
}
