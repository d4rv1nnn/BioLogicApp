using BioLogicNative.Models;
using System.Collections.ObjectModel;
using BioLogicNative; // For ResultPage

namespace MauiProject;

public partial class MainPage : ContentPage
{
    // KnowledgeBase is static, so we don't instantiate it.
    // private KnowledgeBase kb = new KnowledgeBase(); 

    public MainPage()
    {
        InitializeComponent();
        LoadData();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Читаем имя из памяти
        string name = Preferences.Get("UserName", "Биохакер");
        if (UserNameLabel != null) // Safety check
            UserNameLabel.Text = name;
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        // Возможность вернуться к настройкам
        await Navigation.PushAsync(new BioPage());
    }

    private async void LoadData()
    {
        await KnowledgeBase.LoadAsync();
        ApplyPersonalization();
        HormonesList.ItemsSource = KnowledgeBase.Hormones;
    }

    private void ApplyPersonalization()
    {
        // 1. Считываем данные профиля
        string activity = Preferences.Get("ActivityLevel", "Medium"); // Low, Medium, High
        bool hasStress = Preferences.Get("HasStress", false); // Если есть такая настройка
        int age = Preferences.Get("UserAge", 30);

        // 2. Проходимся по гормонам и меняем нормы
        foreach (var hormone in KnowledgeBase.Hormones)
        {
            // --- ЛОГИКА ДЛЯ ТЕСТОСТЕРОНА (ID: h_001) ---
            if (hormone.Id == "h_001")
            {
                // Если Атлет (High Activity) -> повышаем требования к минимуму
                // Атлету нужно больше топлива для восстановления
                if (activity == "High")
                {
                    hormone.Reference.Min += 5.0; // Было 20, станет 25
                    hormone.Description += " (Норма повышена из-за спорта)";
                }
                
                // С возрастом планка чуть падает (реализм), но мы биохакеры, держим её!
                // Но если старше 50, можно чуть смягчить (-2)
                if (age > 50) 
                {
                    hormone.Reference.Min -= 2.0;
                }
            }
            // --- ЛОГИКА ДЛЯ КОРТИЗОЛА (ID: h_004) ---
            if (hormone.Id == "h_004")
            {
                // Если у человека хронический стресс, его "норма" смещена вверх.
                // Мы не хотим пугать его красными флагами на пустом месте.
                if (hasStress)
                {
                    hormone.Reference.Max += 50.0; // Даем "скидку" на стресс
                    hormone.Description += " (Учет фактора стресса)";
                }
            }
        }
    }

    private async void OnAnalyzeClicked(object sender, EventArgs e)
    {
        var problems = new List<Hormone>();
        var allHormones = HormonesList.ItemsSource as List<Hormone>;
        
        if (allHormones == null) return;
        
        // 1. Базовая проверка (выход за рамки)
        foreach (var hormone in allHormones)
        {
            if (double.TryParse(hormone.UserInputValue?.Replace('.', ','), out double value))
            {
                if (value > hormone.Reference.Max || value < hormone.Reference.Min)
                {
                    problems.Add(hormone);
                }
            }
        }

        // 2. УМНАЯ ЛОГИКА (Перекрестный анализ)
        var smartDiagnoses = CheckSmartRules(allHormones);
        problems.InsertRange(0, smartDiagnoses); // Добавляем умные советы в самый верх

        if (problems.Count == 0)
        {
            await DisplayAlert("Отлично", "Все показатели в идеале! Вы — машина.", "OK");
        }
        else
        {
            await Navigation.PushAsync(new ResultPage(problems));
        }
    }

    // "Мозг" приложения
    private List<Hormone> CheckSmartRules(List<Hormone> source)
    {
        var list = new List<Hormone>();

        // Хелпер для получения значения по ID
        double GetVal(string id) 
        {
            var h = source.FirstOrDefault(x => x.Id == id);
            if (h != null && double.TryParse(h.UserInputValue?.Replace('.', ','), out double v)) return v;
            return -1; // Если не найдено или пусто
        }

        double test = GetVal("h_001"); // Тестостерон
        double e2 = GetVal("h_002");   // Эстрадиол
        double prl = GetVal("h_003");  // Пролактин
        double cort = GetVal("h_004"); // Кортизол
        double shbg = GetVal("h_005"); // ГСПГ

        // ПРАВИЛО 1: Ароматизация (Тест низкий, Эстро высокий)
        // Мужчина превращается в женщину гормонально
        if (test > 0 && test < 20 && e2 > 130)
        {
            list.Add(new Hormone
            {
                Name = "⚠️ КРИТИЧЕСКАЯ АРОМАТИЗАЦИЯ",
                Description = "Ваш Тестостерон конвертируется в Эстрадиол. Опасно!",
                Solutions = new Solutions
                {
                    Physics = new() { "Срочно снизить % жира (жир ароматизирует)", "Убрать любой алкоголь" },
                    Supplements = new() { "Ингибиторы Ароматазы (Анастрозол) - к врачу!", "Цинк 50мг", "DIM 200мг" },
                    Psychology = new() { "Контроль истеричности" }
                }
            });
        }

        // ПРАВИЛО 2: Пролактиновый блок
        if (test > 0 && test < 20 && prl > 300)
        {
            list.Add(new Hormone
            {
                Name = "⚠️ ПРОЛАКТИНОВАЯ ЯМА",
                Description = "Высокий Пролактин подавляет выработку Тестостерона.",
                Solutions = new Solutions
                {
                    Physics = new() { "Исключить пиво", "Секс вместо мастурбации", "Сон в темноте" },
                    Supplements = new() { "Каберголин (Достинекс) - строго с врачом", "Витекс", "Витамин B6 (P-5-P)" },
                    Psychology = new() { "Убрать позицию жертвы" }
                }
            });
        }

        // ПРАВИЛО 3: Ловушка ГСПГ (Теста много, но он связан)
        if (test > 25 && shbg > 50)
        {
            list.Add(new Hormone
            {
                Name = "⚠️ СВЯЗАННЫЙ ТЕСТОСТЕРОН",
                Description = "Общий тест высокий, но ГСПГ украл его. Свободный тест низкий.",
                Solutions = new Solutions
                {
                    Physics = new() { "Повысить инсулин (больше углей)", "Больше белка" },
                    Supplements = new() { "Бор (Boron) 10мг", "Корень крапивы (Nettle Root)", "Магний" },
                    Psychology = new() { "Расслабление" }
                }
            });
        }
        
        return list;
    }
}
