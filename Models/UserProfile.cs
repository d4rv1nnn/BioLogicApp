namespace BioLogicNative.Models;

public class UserProfile
{
    public string Name { get; set; }
    public int Age { get; set; }
    public double Weight { get; set; }
    public string Gender { get; set; } // "Male" or "Female"
    public string ActivityLevel { get; set; } // "Low", "Medium", "High"
    public List<string> BadHabits { get; set; } = new();
}
