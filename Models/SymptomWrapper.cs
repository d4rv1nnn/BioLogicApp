namespace BioLogicNative.Models;

public class SymptomWrapper
{
    public string Text { get; set; }
    public string Type { get; set; } // "Deficiency" "Excess"
    public Hormone ParentHormone { get; set; }
}
