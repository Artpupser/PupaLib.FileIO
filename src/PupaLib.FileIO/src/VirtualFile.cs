using System.Collections.Concurrent;

using PupaLib.FileIO.Serializers;

namespace PupaLib.FileIO;

public class VirtualFile {
   private static readonly ConcurrentDictionary<string, VirtualFile> Cache = [];
   private readonly FileInfo _info;

   private VirtualFile(string path) {
      _info = new FileInfo(path);
   }

   public string MyPath => _info.FullName;

   public string Name => _info.Name;

   public VirtualFolder? Parent => VirtualFolder.GetFolder(Path.GetDirectoryName(MyPath));

   public string NameWithoutExtension => Path.GetFileNameWithoutExtension(MyPath);

   public string Extension => _info.Extension;

   public string ExtensionWithoutDot => _info.Extension[1..];

   public long SizeInBytes => _info.Length;

   public DateTime CreationTime => _info.CreationTime;

   public DateTime LastWriteTime => _info.LastWriteTime;

   public bool Exists => _info.Exists;

   #region IO

   public void DeleteMe() {
      if (!Exists) return;
      Cache.Remove(MyPath, out _);
      _info.Delete();
   }

   public void WriteBytes(byte[] content) {
      File.WriteAllBytes(MyPath, content);
   }

   public async Task WriteBytesAsync(byte[] content, CancellationToken cancellationToken = default) {
      await File.WriteAllBytesAsync(MyPath, content, cancellationToken);
   }

   public void WriteString(string content) {
      File.WriteAllText(MyPath, content);
   }

   public async Task WriteStringAsync(string content, CancellationToken cancellationToken = default) {
      await File.WriteAllTextAsync(MyPath, content, cancellationToken);
   }

   public byte[] ReadBytes() {
      return File.ReadAllBytes(MyPath);
   }

   public async Task<byte[]> ReadBytesAsync(CancellationToken cancellationToken = default) {
      return await File.ReadAllBytesAsync(MyPath, cancellationToken);
   }

   public string ReadString() {
      return File.ReadAllText(MyPath);
   }

   public async Task<string> ReadStringAsync(CancellationToken cancellationToken = default) {
      return await File.ReadAllTextAsync(MyPath, cancellationToken);
   }

   public void WriteTContent<T>(T content, ISerializer serializer) where T : class {
      var bytes = serializer.Serialize(content);
      WriteBytes(bytes);
   }

   public async Task WriteTContentAsync<T>(T content, ISerializer serializer,
      CancellationToken cancellationToken = default) where T : class {
      var bytes = serializer.Serialize(content);
      await WriteBytesAsync(bytes, cancellationToken);
   }


   public T ReadTContent<T>(ISerializer serializer) where T : class {
      var bytes = ReadBytes();
      return serializer.Deserialize<T>(bytes);
   }

   public async Task<T> ReadTContentAsync<T>(ISerializer serializer, CancellationToken cancellationToken = default)
      where T : class {
      var bytes = await ReadBytesAsync(cancellationToken);
      return serializer.Deserialize<T>(bytes);
   }

   #endregion

   #region STATIC

   public static FileNotFoundException GetNotFoundException(string path) {
      return new FileNotFoundException($"File not found", path);
   }

   public static VirtualFile? GetFile(string path) {
      return File.Exists(path) ? Cache.GetOrAdd(path, x => new VirtualFile(x)) : null;
   }

   public static VirtualFile GetOrCreate(string path) {
      return Cache.GetOrAdd(path, x => File.Exists(x) ? new VirtualFile(path) : CreateFile(path));
   }

   private static VirtualFile CreateFile(string path) {
      File.Create(path).Dispose();
      var value = new VirtualFile(path);
      Cache.TryAdd(path, value);
      return value;
   }

   #endregion
}