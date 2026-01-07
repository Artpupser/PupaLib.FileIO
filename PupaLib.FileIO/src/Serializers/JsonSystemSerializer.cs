using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace PupaLib.FileIO.Serializers;

public sealed class JsonSystemSerializer: ISerializer {
  public static readonly JsonSerializerOptions Options;

  static JsonSystemSerializer() {
    Options = new JsonSerializerOptions {
      Encoder = JavaScriptEncoder.Create(
        UnicodeRanges.BasicLatin,
        UnicodeRanges.Cyrillic
      )
    };
  }

  #region Inheritance

  public byte[] Serialize<T>(T content) where T : class {
    var bytes = JsonSerializer.SerializeToUtf8Bytes(content, Options);
    return bytes;
  }

  public T Deserialize<T>(byte[] bytes) where T : class {
    return JsonSerializer.Deserialize<T>(bytes, Options)!;
  }

  #endregion
}