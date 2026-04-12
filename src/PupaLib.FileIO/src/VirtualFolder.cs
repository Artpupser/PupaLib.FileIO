using System.Collections.Concurrent;

namespace PupaLib.FileIO;

public class VirtualFolder {
   private static readonly ConcurrentDictionary<string, VirtualFolder> Cache = new();
   private readonly DirectoryInfo _info;

   private VirtualFolder(string path) {
      _info = new DirectoryInfo(path);
   }

   public string MyPath => _info.FullName;

   public VirtualFolder? Parent => GetFolder(_info.Parent?.FullName!);

   public string Name => _info.Name;

   public DateTime CreationTime => _info.CreationTime;

   public DateTime LastWriteTime => _info.LastWriteTime;

   public bool Exists => Directory.Exists(_info.FullName);


   #region Funcs

   public async Task<long> GetSizeInBytesAsync(CancellationToken cancellationToken = default) {
      return await Task.Run(() =>
         _info.EnumerateFiles("*.*", SearchOption.AllDirectories)
            .Sum(f => f.Length), cancellationToken);
   }

   public string BuildPath(string pathIn) {
      return Path.Combine(MyPath, pathIn);
   }


   public VirtualFolder GetOrCreateFolderIn(string pathIn) {
      return GetOrCreateFolder(BuildPath(pathIn));
   }

   public VirtualFolder? GetFolderIn(string pathIn) {
      return GetFolder(BuildPath(pathIn));
   }

   public VirtualFile GetOrCreateFileIn(string pathIn) {
      return VirtualFile.GetOrCreate(BuildPath(pathIn));
   }

   public VirtualFile? GetFileIn(string pathIn) {
      return VirtualFile.GetFile(BuildPath(pathIn));
   }

   public IEnumerable<VirtualFile> EnumerateFiles(SearchOption option) {
      return _info.EnumerateFiles("*", option)
         .Select(x => VirtualFile.GetFile(x.FullName))
         .Where(x => x != null)!;
   }

   public IEnumerable<VirtualFolder> EnumerateFolders(SearchOption option) {
      return _info.EnumerateDirectories("*", option)
         .Select(x => GetFolder(x.FullName)!);
   }

   public int GetDirectoriesCount(SearchOption option = SearchOption.TopDirectoryOnly) {
      return _info.GetDirectories("*", option).Length;
   }

   public int GetFilesCount(SearchOption option = SearchOption.TopDirectoryOnly) {
      return _info.GetFiles("*", option).Length;
   }

   public void DeleteMe(bool full = true) {
      if (!Exists) return;
      Cache.Remove(MyPath, out _);
      Directory.Delete(MyPath, full);
   }

   #endregion

   #region STATIC

   public static DirectoryNotFoundException GetNotFoundException(string path) {
      return new DirectoryNotFoundException($"Directory not found {path}");
   }

   public static VirtualFolder? GetFolder(string? path) {
      return Directory.Exists(path) ? Cache.GetOrAdd(path, s => new VirtualFolder(s)) : null;
   }

   public static VirtualFolder GetOrCreateFolder(string path) {
      return Cache.GetOrAdd(path, s => Directory.Exists(s) ? new VirtualFolder(path) : CreateFolder(path));
   }

   private static VirtualFolder CreateFolder(string path) {
      Directory.CreateDirectory(path);
      var value = new VirtualFolder(path);
      Cache.TryAdd(path, value);
      return value;
   }

   #endregion
}