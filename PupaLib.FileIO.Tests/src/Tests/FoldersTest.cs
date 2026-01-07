namespace PupaLib.FileIO.Tests.Tests;

[CollectionDefinition("FoldersTest collection", DisableParallelization = true)]
public class FoldersTest {
  public const string FolderName = "test_folder";
  public const string MessageErrorCorrect = "Folder is not correct";

  private bool FolderIsCorrect(VirtualFolder? folder) {
    return folder != null && folder.Exists && folder.Name != string.Empty;
  }

  [Fact(DisplayName = "Root folder is correct")]
  public void Should_CheckRootFolder_When_FolderIsCorrect() {
    Assert.True(VirtualIo.RootFolder.Exists);
    Assert.NotEmpty(VirtualIo.RootPath);
  }

  [Fact(DisplayName = "Creating folder")]
  public void Should_CreateFolder_When_ValidPath() {
    var folder = VirtualIo.RootFolder.CreateFolderIn(FolderName);
    Assert.True(FolderIsCorrect(folder), MessageErrorCorrect);
  }

  [Fact(DisplayName = "Load folder")]
  public void Should_LoadFolder_When_ValidPathAndAlreadyCreated() {
    VirtualIo.RootFolder.CreateFolderIn(FolderName);
    var folder = VirtualIo.RootFolder.GetFolderIn(FolderName);
    Assert.True(FolderIsCorrect(folder), MessageErrorCorrect);
  }

  [Fact(DisplayName = "Fill and removes files")]
  public void Should_FillFiles_When_ValidPathAndAlreadyCreated() {
    VirtualIo.RootFolder.CreateFolderIn(FolderName);
    var folder = VirtualIo.RootFolder.GetFolderIn(FolderName);
    Assert.True(FolderIsCorrect(folder), MessageErrorCorrect);
    for(var i = 0; i < 5; i++) {
      var file = folder!.CreateFileIn($"file{i}.txt");
      Assert.True(file.Exists);
      file.DeleteMe();
      Assert.False(file.Exists);
    }

    folder!.DeleteMe();
  }

  [Fact(DisplayName = "Delete folder")]
  public void Should_DeleteFolder_When_ValidPathAndAlreadyCreated() {
    VirtualIo.RootFolder.CreateFolderIn(FolderName);
    var folder = VirtualIo.RootFolder.GetFolderIn(FolderName);
    Assert.True(FolderIsCorrect(folder), MessageErrorCorrect);
    folder!.DeleteMe();
    Assert.False(folder.Exists, $"Folder not deleted, {folder.MyPath}");
  }
}