using PupaLib.FileIO.Serializers;
using PupaLib.FileIO.Tests.Models;

namespace PupaLib.FileIO.Tests.Tests;

[CollectionDefinition("FileTestsAsync collection", DisableParallelization = true)]
public class FileTestAsync {
   public const string FileName = "test_file_async.txt";

   private static bool FileIsCorrect(VirtualFile? file) {
      return file != null && file.Exists && file.Name != string.Empty;
   }

   [Theory(DisplayName = "Write and read file")]
   [InlineData("Hello world!")]
   [InlineData("New year!")]
   [InlineData("Happy me!")]
   public async Task Should_WriteAndReadFile_When_ValidPathAndAlreadyCreated(string content,
      CancellationToken cancellationToken = default) {
      var file = VirtualIo.RootFolder.GetOrCreateFileIn(FileName);
      var fileCorrect = FileIsCorrect(file);
      Assert.True(fileCorrect, "File was not correct");
      if (!fileCorrect) return;
      await file!.WriteStringAsync(content, cancellationToken);
      var reads = await file.ReadStringAsync(cancellationToken);
      Assert.True(reads == content, $"File is empty, {reads} != {content}");
   }

   [Theory(DisplayName = "Write and read file")]
   [InlineData("name1", "lastname1")]
   [InlineData("name2", "lastname2")]
   [InlineData("name3", "lastname3")]
   public async Task Should_WriteAndReadFileT_When_ValidPathAndAlreadyCreated(string name,
      string lastname,
      CancellationToken cancellationToken = default) {
      var file = VirtualIo.RootFolder.GetOrCreateFileIn(FileName);
      var fileCorrect = FileIsCorrect(file);
      Assert.True(fileCorrect, "File was not correct");
      if (!fileCorrect) return;
      var content = new TestObjectSecond(name, lastname);
      await file!.WriteTContentAsync(content, new JsonSystemSerializer(), cancellationToken);
      var reads = await file.ReadTContentAsync<TestObjectSecond>(new JsonSystemSerializer(), cancellationToken);
      Assert.True(reads.Name == content.Name && reads.LastName == content.LastName,
         $"File is empty, {reads} != {content}");
   }
}