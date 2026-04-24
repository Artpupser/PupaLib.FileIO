namespace PupaLib.FileIO.Tests.Tests;

[CollectionDefinition("FoldersTest collection", DisableParallelization = true)]
public class FoldersTest {
   private const string FolderName = "test_folder";
   private const string MessageErrorCorrect = "Folder is not correct";

   [Fact(DisplayName = "Root folder is correct")]
   public void Should_CheckRootFolder_When_FolderIsCorrect() {
      Assert.True(VirtualIo.RootFolder.Exists);
      Assert.NotEmpty(VirtualIo.RootPath);
   }

   [Fact(DisplayName = "Creating folder")]
   public void Should_CreateFolder_When_ValidPath() {
      var folderOption = VirtualIo.RootFolder.GetOrCreateFolderIn(FolderName);
      Assert.True(folderOption, MessageErrorCorrect);
   }

   [Fact(DisplayName = "Load folder")]
   public void Should_LoadFolder_When_ValidPathAndAlreadyCreated() {
      VirtualIo.RootFolder.GetOrCreateFolderIn(FolderName);
      var folderOption = VirtualIo.RootFolder.GetFolderIn(FolderName);
      Assert.True(folderOption, MessageErrorCorrect);
   }

   [Fact(DisplayName = "Fill and removes files")]
   public void Should_FillFiles_When_ValidPathAndAlreadyCreated() {
      VirtualIo.RootFolder.GetOrCreateFolderIn(FolderName);
      var folderOption = VirtualIo.RootFolder.GetFolderIn(FolderName);
      Assert.True(folderOption, MessageErrorCorrect);
      if (!folderOption.Out(out var folder))
         return;
      for (var i = 0; i < 5; i++) {
         var fileOption = folder.GetOrCreateFileIn($"file{i}.txt");
         Assert.True(fileOption, $"File 'file{i}.txt' read failed");
         if (!fileOption.Out(out var file))
            return;
         Assert.True(file.Exists);
         file.DeleteMe();
         Assert.False(file.Exists);
      }

      folder.DeleteMe();
   }

   [Fact(DisplayName = "Delete folder")]
   public void Should_DeleteFolder_When_ValidPathAndAlreadyCreated() {
      VirtualIo.RootFolder.GetOrCreateFolderIn(FolderName);
      var folderOption = VirtualIo.RootFolder.GetFolderIn(FolderName);
      Assert.True(folderOption, MessageErrorCorrect);
      if (!folderOption.Out(out var folder))
         return;
      folder.DeleteMe();
      Assert.False(folder.Exists, $"Folder not deleted, {folder.MyPath}");
   }
}