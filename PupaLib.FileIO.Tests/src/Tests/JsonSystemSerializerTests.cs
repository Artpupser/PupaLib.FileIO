using PupaLib.FileIO.Serializers;
using PupaLib.FileIO.Tests.Models;

namespace PupaLib.FileIO.Tests.Tests;

[CollectionDefinition("JsonSystemSerializer collection", DisableParallelization = true)]
public class JsonSystemSerializerTests {
  [Fact(DisplayName = "Serialize object valid")]
  public void Should_SerializeObject_When_Valid() {
    var serializer = new JsonSystemSerializer();
    var obj = new TestObject("name", [1, 2, 3, 4, 5], "lastname");
    var bytes = serializer.Serialize(obj);
    Assert.True(bytes.Length > 4);
  }

  [Fact(DisplayName = "Serialize object not valid")]
  public void Should_SerializeObject_When_NotValid() {
    var serializer = new JsonSystemSerializer();
    TestObject obj = null!;
    var bytes = serializer.Serialize(obj);
    Assert.True(bytes.Length == 4);
  }

  [Fact(DisplayName = "Deserialize object valid")]
  public void Should_DeserializeObject_When_Valid() {
    var serializer = new JsonSystemSerializer();
    var obj = new TestObject("name", [1, 2, 3, 4, 5], "lastname");
    var bytes = serializer.Serialize(obj);
    var content = serializer.Deserialize<TestObject>(bytes);
    Assert.True(obj.Name == content.Name && obj.LastName == content.LastName &&
                obj.Numbers.All(x => content.Numbers.Any(y => y == x)));
  }

  [Fact(DisplayName = "Deserialize object not valid")]
  public void Should_DeserializeObject_When_NotValid() {
    var serializer = new JsonSystemSerializer();
    TestObject obj = null!;
    var bytes = serializer.Serialize(obj);
    var content = serializer.Deserialize<TestObject>(bytes);
    Assert.Null(content);
  }
}