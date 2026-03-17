namespace PupaLib.FileIO.Example.Extensions;

/// <summary>
/// Extension methods for <see cref="VirtualFile"/>.
/// </summary>
public static class VirtualFileExtensions {
   /// <summary>
   /// Returns formatted file information (path, name, size, dates, exists status).
   /// </summary>
   /// <param name="file">The file to get info for</param>
   /// <returns>Multi-line formatted string with file details</returns>
   public static string GetInfo(this VirtualFile file) {
      return
         $"FileInfo: {{\n\tPath: {file.MyPath}\n\tName: {file.Name}\n\tCreation: {file.CreationTime}\n\tBytes: {file.SizeInBytes}\n\tExists: {file.Exists}\n}}";
   }
}