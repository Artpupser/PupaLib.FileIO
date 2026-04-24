using System.Text.Json.Serialization;

namespace PupaLib.FileIO.Tests.Models;

[Serializable]
[method: JsonConstructor]
public sealed class TestObjectSecond(string name, string lastname) {
   [JsonPropertyName("Name")] public string Name { get; } = name;
   [JsonPropertyName("Lastname")] public string LastName { get; } = lastname;
}