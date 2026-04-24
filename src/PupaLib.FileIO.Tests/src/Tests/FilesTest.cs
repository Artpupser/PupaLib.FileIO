using PupaLib.FileIO.Serializers;
using PupaLib.FileIO.Tests.Models;

namespace PupaLib.FileIO.Tests.Tests;

[CollectionDefinition("FileTests collection", DisableParallelization = true)]
public class FilesTest {
   private const string FileName = "test_file.txt";


   [Fact(DisplayName = "Creating file")]
   public void Should_CreateFile_When_ValidPath() {
      var file = VirtualIo.RootFolder.GetOrCreateFileIn(FileName);
      Assert.True(file.Exists, "File was not correct");
   }

   [Fact(DisplayName = "Load file")]
   public void Should_LoadFile_When_ValidPathAndAlreadyCreated() {
      VirtualIo.RootFolder.GetOrCreateFileIn(FileName);
      var fileOption = VirtualIo.RootFolder.GetFileIn(FileName);
      Assert.True(fileOption, "File was not correct");
   }

   [Theory(DisplayName = "Write and read file")]
   [InlineData("Hello world!")]
   [InlineData("New year!")]
   [InlineData("Happy me!")]
   public void Should_WriteAndReadFile_When_ValidPathAndAlreadyCreated(string content) {
      VirtualIo.RootFolder.GetOrCreateFileIn(FileName);
      var fileOption = VirtualIo.RootFolder.GetFileIn(FileName);
      Assert.True(fileOption, "File was not correct");
      if (!fileOption.Out(out var file))
         return;
      file!.WriteString(content);
      var readOperationOption = file.ReadString();
      Assert.True(readOperationOption, $"Reading operation is bad");
      if (!readOperationOption.Out(out var str))
         return;
      Assert.True(str == content, $"File is empty, {readOperationOption} != {content}");
   }

   [Theory(DisplayName = "Write and read file")]
   [InlineData("name1", "lastname1")]
   [InlineData("name2", "lastname2")]
   [InlineData("name3", "lastname3")]
   public void Should_WriteAndReadFileT_When_ValidPathAndAlreadyCreated(string name,
      string lastname) {
      var fileOption = VirtualIo.RootFolder.GetOrCreateFileIn(FileName);
      Assert.True(fileCorrect, "File was not correct");
      if (!fileCorrect) return;
      var content = new TestObjectSecond(name, lastname);
      file!.WriteTContent(content, new JsonSystemSerializer());
      var reads = file.ReadTContent<TestObjectSecond>(new JsonSystemSerializer());
      Assert.True(reads.Name == content.Name && reads.LastName == content.LastName,
         $"File is empty, {reads} != {content}");
   }

   [Fact(DisplayName = "Delete file")]
   public void Should_DeleteFile_When_ValidPathAndAlreadyCreated() {
      VirtualIo.RootFolder.GetOrCreateFileIn(FileName);
      var file = VirtualIo.RootFolder.GetFileIn(FileName);
      var fileCorrect = FileIsCorrect(file);
      Assert.True(fileCorrect);
      if (!fileCorrect) return;
      file!.DeleteMe();
      Assert.False(file.Exists);
   }
}