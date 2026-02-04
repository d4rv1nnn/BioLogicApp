using System.Text.Json;

namespace BioLogicNative.Models;

public static class KnowledgeBase
{
    public static List<Hormone> Hormones { get; set; } = new();

    public static async Task LoadAsync()
    {
        try 
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("app_data.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var data = JsonSerializer.Deserialize<KnowledgeBaseData>(contents, options);
            
            Hormones = data?.HormoneMasterData ?? new List<Hormone>();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Файл app_data.json не найден или поврежден: " + ex.Message, "ОК");
        }
    }
}
