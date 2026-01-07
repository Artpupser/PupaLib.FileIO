namespace PupaLib.FileIO;

public class VirtualFolder {
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

  public long SizeInBytes => EnumerateFiles(SearchOption.AllDirectories).Sum(x => x.SizeInBytes);

  #region Funcs

  public string BuildPath(string pathIn) {
    return Path.Combine(MyPath, pathIn);
  }

  public VirtualFolder CreateFolderIn(string pathIn) {
    return CreateFolder(BuildPath(pathIn));
  }

  public VirtualFolder? GetFolderIn(string pathIn) {
    return GetFolder(BuildPath(pathIn));
  }

  public VirtualFile CreateFileIn(string pathIn) {
    return VirtualFile.CreateFile(BuildPath(pathIn));
  }

  public VirtualFile? GetFileIn(string pathIn) {
    return VirtualFile.GetFile(BuildPath(pathIn));
  }

  public IEnumerable<VirtualFile> EnumerateFiles(SearchOption option) {
    return _info.EnumerateFiles("*", option)
      .Select(x => VirtualFile.GetFile(x.FullName)!);
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

  public void DeleteMe() {
    Directory.Delete(MyPath, true);
  }

  #endregion

  #region Init

  public static DirectoryNotFoundException GetNotFoundException(string path) {
    return new DirectoryNotFoundException($"Directory not found {path}");
  }

  public static VirtualFolder? GetFolder(string path) {
    return Directory.Exists(path) ? new VirtualFolder(path) : null;
  }

  public static VirtualFolder CreateFolder(string path) {
    Directory.CreateDirectory(path);
    return new VirtualFolder(path);
  }

  #endregion
}