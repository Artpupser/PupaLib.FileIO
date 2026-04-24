using System.Text.Json.Serialization;

namespace PupaLib.FileIO.Tests.Models;

[Serializable]
[method: JsonConstructor]
public sealed class TestObjectFirst(string name, List<int> numbers, string lastname) {
   [JsonPropertyName("Name")] public string Name { get; } = name;
   [JsonPropertyName("Numbers")] public List<int> Numbers { get; } = numbers;
   [JsonPropertyName("Lastname")] public string LastName { get; } = lastname;
}