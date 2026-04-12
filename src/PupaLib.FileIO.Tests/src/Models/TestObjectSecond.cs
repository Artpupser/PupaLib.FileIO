using System.Text.Json.Serialization;

namespace PupaLib.FileIO.Tests.Models;

[Serializable]
public sealed class TestObjectSecond {
   [JsonConstructor]
   public TestObjectSecond(string name, string lastname) {
      Name = name;
      LastName = lastname;
   }

   [JsonPropertyName("Name")] public string Name { get; }
   [JsonPropertyName("Lastname")] public string LastName { get; }
}