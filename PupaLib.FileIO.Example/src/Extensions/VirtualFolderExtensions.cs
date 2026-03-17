namespace PupaLib.FileIO.Example.Extensions;

/// <summary>
/// Extension methods for <see cref="VirtualFolder"/>.
/// </summary>
public static class VirtualFolderExtensions {
   /// <summary>
   /// Returns formatted folder information (path, name, creation date, exists status).
   /// </summary>
   /// <param name="folder">The folder to get info for</param>
   /// <returns>Multi-line formatted string with folder details</returns>
   public static string GetInfo(this VirtualFolder folder) {
      return
         $"FolderInfo: {{\n\tPath: {folder.MyPath}\n\tName: {folder.Name}\n\tCreation: {folder.CreationTime}\n\tExists: {folder.Exists}\n}}";
   }
}