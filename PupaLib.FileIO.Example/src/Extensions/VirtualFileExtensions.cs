namespace PupaLib.FileIO.Example.Extensions;

public static class VirtualFileExtensions {
  public static string GetInfo(this VirtualFile file) {
    return
      $"FileInfo: {{\n\tPath: {file.MyPath}\n\tName: {file.Name}\n\tCreation: {file.CreationTime}\n\tBytes: {file.SizeInBytes}\n\tExists: {file.Exists}\n}}";
  }
}