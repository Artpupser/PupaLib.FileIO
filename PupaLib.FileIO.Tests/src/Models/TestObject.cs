using System.Text.Json.Serialization;

namespace PupaLib.FileIO.Tests.Models;

[System.Serializable]
public sealed class TestObject {
  private string _name;
  private List<int> _numbers;
  private string _lastname;

  [JsonConstructor]
  public TestObject(string name, List<int> numbers, string lastname) {
    _name = name;
    _numbers = numbers;
    _lastname = lastname;
  }

  [JsonPropertyName("Name")]
  public string Name => _name;

  [JsonPropertyName("Numbers")]
  public List<int> Numbers => _numbers;

  [JsonPropertyName("Lastname")]
  public string LastName => _lastname;
}