namespace PupaLib.FileIO;

/// <summary>
/// Provides global access to the application file system through virtual wrappers
/// </summary>
public static class VirtualIo {
   /// <summary>
   /// Root directory of the executable application (BaseDirectory)
   /// </summary>
   public static readonly string RootPath = AppDomain.CurrentDomain.BaseDirectory;

   /// <summary>
   /// Root folder of the application.
   /// Automatically initialized from <see cref="RootPath"/>.
   /// Throws <see cref="DirectoryNotFoundException"/> if the directory is not accessible
   /// </summary>
   public static readonly VirtualFolder RootFolder =
      VirtualFolder.GetFolder(RootPath) ?? throw VirtualFolder.GetNotFoundException(RootPath);
}