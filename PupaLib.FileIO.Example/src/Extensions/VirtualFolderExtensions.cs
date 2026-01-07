namespace PupaLib.FileIO.Example.Extensions;

public static class VirtualFolderExtensions {
  public static string GetInfo(this VirtualFolder folder) {
    return
      $"FolderInfo: {{\n\tPath: {folder.MyPath}\n\tName: {folder.Name}\n\tCreation: {folder.CreationTime}\n\tBytes: {folder.SizeInBytes}\n\tExists: {folder.Exists}\n}}";
  }
}