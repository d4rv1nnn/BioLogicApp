using System.Text.Json.Serialization;

namespace BioLogicNative.Models;

public class KnowledgeBaseData
{
    [JsonPropertyName("hormone_master_data")]
    public List<Hormone> HormoneMasterData { get; set; }
}

public class Hormone
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    [JsonPropertyName("reference")]
    public ReferenceRange Reference { get; set; }

    [JsonPropertyName("solutions")]
    public Solutions Solutions { get; set; } = new();

    [JsonIgnore]
    public string UserInputValue { get; set; }

    [JsonIgnore]
    public bool IsExpanded { get; set; }
}

public class ReferenceRange
{
    [JsonPropertyName("min")]
    public double Min { get; set; }

    [JsonPropertyName("max")]
    public double Max { get; set; }

    [JsonPropertyName("unit")]
    public string Unit { get; set; }
}

public class Solutions
{
    [JsonPropertyName("physics")]
    public List<string> Physics { get; set; } = new();

    [JsonPropertyName("psychology")]
    public List<string> Psychology { get; set; } = new();

    [JsonPropertyName("supplements")]
    public List<string> Supplements { get; set; } = new();
}
