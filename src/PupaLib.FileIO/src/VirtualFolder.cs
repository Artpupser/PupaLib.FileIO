using System.Collections.Concurrent;

using PupaLib.Core;

namespace PupaLib.FileIO;

public class VirtualFolder {
   private static readonly ConcurrentDictionary<string, VirtualFolder> Cache = new();
   private readonly DirectoryInfo _info;

   private VirtualFolder(string path) {
      _info = new DirectoryInfo(path);
   }

   public string MyPath => _info.FullName;

   public Option<VirtualFolder> Parent => GetFolder(_info.Parent?.FullName!);

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


   public Option<VirtualFolder> GetOrCreateFolderIn(string pathIn) {
      return GetOrCreateFolder(BuildPath(pathIn));
   }

   public Option<VirtualFolder> GetFolderIn(string pathIn) {
      return GetFolder(BuildPath(pathIn));
   }

   public Option<VirtualFile> GetOrCreateFileIn(string pathIn) {
      return VirtualFile.GetOrCreate(BuildPath(pathIn));
   }

   public Option<VirtualFile> GetFileIn(string pathIn) {
      return VirtualFile.GetFile(BuildPath(pathIn));
   }

   public IEnumerable<VirtualFile> EnumerateFiles(SearchOption option) {
      var contents = new List<VirtualFile>(64);
      foreach (var fileInfo in _info.EnumerateFiles("*", option))
         if (VirtualFile.GetFile(fileInfo.FullName) is { Success: true } file)
            contents.Add(file.Content);
      return contents;
   }

   public IEnumerable<VirtualFolder> EnumerateFolders(SearchOption option) {
      var contents = new List<VirtualFolder>(64);
      foreach (var directoryInfo in _info.EnumerateDirectories("*", option))
         if (GetFolder(directoryInfo.FullName) is { Success: true } folder)
            contents.Add(folder.Content);
      return contents;
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

   public static DirectoryNotFoundException NotFoundException(string path) {
      return new DirectoryNotFoundException($"Directory not found {path}");
   }

   public static Option<VirtualFolder> GetFolder(string? path) {
      return Directory.Exists(path)
         ? Option<VirtualFolder>.Ok(Cache.GetOrAdd(path, s => new VirtualFolder(s)))
         : Option<VirtualFolder>.Fail();
   }

   public static Option<VirtualFolder> GetOrCreateFolder(string path) {
      return Option<VirtualFolder>.Ok(Cache.GetOrAdd(path,
         keyPath => Directory.Exists(keyPath) ? new VirtualFolder(path) : CreateFolder(path).Content));
   }

   private static Option<VirtualFolder> CreateFolder(string path) {
      try {
         Directory.CreateDirectory(path);
         var value = new VirtualFolder(path);
         Cache.TryAdd(path, value);
         return Option<VirtualFolder>.Ok(value);
      } catch (Exception e) {
         return Option<VirtualFolder>.Fail();
      }
   }

   #endregion
}