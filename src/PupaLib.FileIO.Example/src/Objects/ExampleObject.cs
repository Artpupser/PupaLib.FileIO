using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PupaLib.FileIO.Example.Objects;

/// <summary>
/// Sample object for JSON serialization demos.
/// </summary>
[System.Serializable]
public sealed class ExampleObject {
   /// <summary>
   /// Creates example object with name, lastname and numbers list.
   /// Used by JSON deserializer.
   /// </summary>
   /// <param name="name">Person's first name</param>
   /// <param name="lastname">Person's last name</param>
   /// <param name="numbers">List of numbers</param>
   [JsonConstructor]
   public ExampleObject(string name, string lastname, List<int> numbers) {
      Name = name;
      Lastname = lastname;
      Numbers = numbers;
   }

   /// <summary>
   /// Returns formatted object info: Name, Lastname, Numbers.
   /// </summary>
   /// <returns>Multi-line string with object details</returns>
   public override string ToString() {
      return $"Name: {Name}\nLastname: {Lastname}\nNumbers: {string.Join(',', Numbers)}";
   }

   /// <summary>
   /// Person's first name.
   /// </summary>
   [JsonPropertyName("Name")]
   public string Name { get; init; }

   /// <summary>
   /// Person's last name.
   /// </summary>
   [JsonPropertyName("Lastname")]
   public string Lastname { get; init; }

   /// <summary>
   /// List of integer numbers.
   /// </summary>
   [JsonPropertyName("Numbers")]
   public List<int> Numbers { get; init; }
}