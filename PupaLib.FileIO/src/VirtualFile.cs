using PupaLib.FileIO.Serializers;

namespace PupaLib.FileIO;

public class VirtualFile {
  private readonly FileInfo _info;

  private VirtualFile(string path) {
    _info = new FileInfo(path);
  }

  public string MyPath => _info.FullName;

  public string Name => _info.Name;

  public VirtualFolder Parent => VirtualFolder.GetFolder(MyPath)!;

  public string NameWithoutExtension => Path.GetFileNameWithoutExtension(MyPath);

  public string Extension => _info.Extension;

  public string ExtensionWithoutDot => _info.Extension[1..];

  public long SizeInBytes => _info.Length;

  public DateTime CreationTime => _info.CreationTime;

  public DateTime LastWriteTime => _info.LastWriteTime;

  public bool Exists => _info.Exists;

  #region Funcs

  public void DeleteMe() {
    _info.Delete();
  }

  public void WriteBytes(byte[] content) {
    File.WriteAllBytes(MyPath, content);
  }

  public async Task WriteBytesAsync(byte[] content) {
    await File.WriteAllBytesAsync(MyPath, content);
  }

  public void WriteString(string content) {
    File.WriteAllText(MyPath, content);
  }

  public async Task WriteStringAsync(string content) {
    await File.WriteAllTextAsync(MyPath, content);
  }

  public byte[] ReadBytes() {
    return File.ReadAllBytes(MyPath);
  }

  public async Task<byte[]> ReadBytesAsync() {
    return await File.ReadAllBytesAsync(MyPath);
  }

  public string ReadString() {
    return File.ReadAllText(MyPath);
  }

  public async Task<string> ReadStringAsync() {
    return await File.ReadAllTextAsync(MyPath);
  }

  public void WriteTContent<T>(T content, ISerializer serializer) where T : class {
    var bytes = serializer.Serialize(content);
    WriteBytes(bytes);
  }

  public T ReadTContent<T>(ISerializer serializer) where T : class {
    var bytes = ReadBytes();
    return serializer.Deserialize<T>(bytes);
  }

  #endregion

  #region Init

  public static FileNotFoundException GetNotFoundException(string path) {
    return new FileNotFoundException($"File not found {path}");
  }

  public static VirtualFile? GetFile(string path) {
    return File.Exists(path) ? new VirtualFile(path) : null;
  }

  public static VirtualFile CreateFile(string path) {
    File.WriteAllText(path, string.Empty);
    return new VirtualFile(path);
  }

  #endregion
}