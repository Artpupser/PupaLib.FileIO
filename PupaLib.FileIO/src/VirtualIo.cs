namespace PupaLib.FileIO;

public static class VirtualIo {
  public static readonly string RootPath = AppDomain.CurrentDomain.BaseDirectory;

  public static readonly VirtualFolder RootFolder =
    VirtualFolder.GetFolder(RootPath) ?? throw VirtualFolder.GetNotFoundException(RootPath);
}