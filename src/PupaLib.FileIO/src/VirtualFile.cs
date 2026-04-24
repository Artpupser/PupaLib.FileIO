using System.Collections.Concurrent;

using PupaLib.Core;
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

   public Option<VirtualFolder> Parent => VirtualFolder.GetFolder(Path.GetDirectoryName(MyPath));

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

   public Option<byte[]> ReadBytes() {
      try {
         return Option<byte[]>.Ok(File.ReadAllBytes(MyPath));
      } catch {
         return Option<byte[]>.Fail();
      }
   }

   public async Task<Option<byte[]>> ReadBytesAsync(CancellationToken cancellationToken = default) {
      try {
         var bytes = await File.ReadAllBytesAsync(MyPath, cancellationToken);
         return Option<byte[]>.Ok(bytes);
      } catch {
         return Option<byte[]>.Fail();
      }
   }

   public Option<string> ReadString() {
      try {
         return Option<string>.Ok(File.ReadAllText(MyPath));
      } catch {
         return Option<string>.Fail();
      }
   }

   public async Task<Option<string>> ReadStringAsync(CancellationToken cancellationToken = default) {
      try {
         return Option<string>.Ok(await File.ReadAllTextAsync(MyPath, cancellationToken));
      } catch {
         return Option<string>.Fail();
      }
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


   public Option<T> ReadTContent<T>(ISerializer serializer) where T : class {
      try {
         var bytesOption = ReadBytes();
         return !bytesOption.Out(out var bytes) ? Option<T>.Fail() : Option<T>.Ok(serializer.Deserialize<T>(bytes));
      } catch {
         return Option<T>.Fail();
      }
   }

   public async Task<Option<T>> ReadTContentAsync<T>(ISerializer serializer,
      CancellationToken cancellationToken = default)
      where T : class {
      try {
         var bytesOption = await ReadBytesAsync(cancellationToken);
         return !bytesOption.Out(out var bytes) ? Option<T>.Fail() : Option<T>.Ok(serializer.Deserialize<T>(bytes));
      } catch {
         return Option<T>.Fail();
      }
   }

   #endregion

   #region STATIC

   public static FileNotFoundException NotFoundException(string path) {
      return new FileNotFoundException($"File not found", path);
   }

   public static Option<VirtualFile> GetFile(string path) {
      return !File.Exists(path)
         ? Option<VirtualFile>.Fail()
         : Option<VirtualFile>.Ok(Cache.GetOrAdd(path, x => new VirtualFile(x)));
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