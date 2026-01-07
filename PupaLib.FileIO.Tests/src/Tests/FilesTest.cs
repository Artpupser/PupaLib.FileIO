using Xunit.Abstractions;

namespace PupaLib.FileIO.Tests.Tests;

[CollectionDefinition("FileTests collection", DisableParallelization = true)]
public class FilesTest {
  public const string FileName = "test_file.txt";

  private bool FileIsCorrect(VirtualFile? file) {
    return file != null && file.Exists && file.Name != string.Empty;
  }

  [Fact(DisplayName = "Creating file")]
  public void Should_CreateFile_When_ValidPath() {
    var file = VirtualIo.RootFolder.CreateFileIn(FileName);
    Assert.True(FileIsCorrect(file), "File was not correct");
  }

  [Fact(DisplayName = "Load file")]
  public void Should_LoadFile_When_ValidPathAndAlreadyCreated() {
    VirtualIo.RootFolder.CreateFileIn(FileName);
    var file = VirtualIo.RootFolder.GetFileIn(FileName);
    Assert.True(FileIsCorrect(file), "File was not correct");
  }

  [Theory(DisplayName = "Write and read file")]
  [InlineData("Hello world!")]
  [InlineData("New year!")]
  [InlineData("Happy me!")]
  public void Should_WriteAndReadFile_When_ValidPathAndAlreadyCreated(string content) {
    VirtualIo.RootFolder.CreateFileIn(FileName);
    var file = VirtualIo.RootFolder.GetFileIn(FileName);
    var fileCorrect = FileIsCorrect(file);
    Assert.True(fileCorrect, "File was not correct");
    if(!fileCorrect) return;
    file!.WriteString(content);
    var readed = file.ReadString();
    Assert.True(readed == content, $"File is empty, {readed} != {content}");
  }

  [Fact(DisplayName = "Delete file")]
  public void Should_DeleteFile_When_ValidPathAndAlreadyCreated() {
    VirtualIo.RootFolder.CreateFileIn(FileName);
    var file = VirtualIo.RootFolder.GetFileIn(FileName);
    var fileCorrect = FileIsCorrect(file);
    Assert.True(fileCorrect);
    if(!fileCorrect) return;
    file!.DeleteMe();
    Assert.False(file.Exists);
  }
}