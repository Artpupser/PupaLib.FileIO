using System.Text.Json.Serialization;

namespace PupaLib.FileIO.Tests.Models;

[Serializable]
public sealed class TestObjectFirst {
   [JsonConstructor]
   public TestObjectFirst(string name, List<int> numbers, string lastname) {
      Name = name;
      Numbers = numbers;
      LastName = lastname;
   }

   [JsonPropertyName("Name")] public string Name { get; }
   [JsonPropertyName("Numbers")] public List<int> Numbers { get; }
   [JsonPropertyName("Lastname")] public string LastName { get; }
}