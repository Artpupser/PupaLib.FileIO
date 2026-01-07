using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PupaLib.FileIO.Example.Objects;

[System.Serializable]
public sealed class ExampleObject {
  private string _name;
  private List<int> _numbers;
  private string _lastname;

  [JsonConstructor]
  public ExampleObject(string name, string lastname, List<int> numbers) {
    _name = name;
    _lastname = lastname;
    _numbers = numbers;
  }

  public override string ToString() {
    return $"Name: {_name}\nLastname: {_lastname}\nNumbers: {string.Join(',', _numbers)}";
  }

  [JsonPropertyName("Name")]
  public string Name => _name;

  [JsonPropertyName("Numbers")]
  public List<int> Numbers => _numbers;

  [JsonPropertyName("Lastname")]
  public string Lastname => _lastname;
}