using PupaLib.FileIO.Serializers;
using PupaLib.FileIO.Tests.Models;

namespace PupaLib.FileIO.Tests.Tests;

[CollectionDefinition("FileTestsAsync collection", DisableParallelization = true)]
public class FileTestAsync {
   private const string FileName = "test_file_async.txt";

   [Theory(DisplayName = "Write and read file")]
   [InlineData("Hello world!")]
   [InlineData("New year!")]
   [InlineData("Happy me!")]
   public async Task Should_WriteAndReadFile_When_ValidPathAndAlreadyCreated(string content,
      CancellationToken cancellationToken = default) {
      var fileOption = VirtualIo.RootFolder.GetOrCreateFileIn(FileName);
      Assert.True(fileOption, "File was not correct");
      if (!fileOption.Out(out var file)) return;
      await file.WriteStringAsync(content, cancellationToken);
      var readOperationOption = await file.ReadStringAsync(cancellationToken);
      if (!readOperationOption.Out(out var reads))
         return;
      Assert.True(reads == content, $"File is empty, {reads} != {content}");
   }

   [Theory(DisplayName = "Write and read file")]
   [InlineData("name1", "lastname1")]
   [InlineData("name2", "lastname2")]
   [InlineData("name3", "lastname3")]
   public async Task Should_WriteAndReadFileT_When_ValidPathAndAlreadyCreated(string name,
      string lastname,
      CancellationToken cancellationToken = default) {
      var fileOption = VirtualIo.RootFolder.GetOrCreateFileIn(FileName);
      Assert.True(fileOption, "File was not correct");
      if (!fileOption.Out(out var file)) return;
      var content = new TestObjectSecond(name, lastname);
      await file.WriteTContentAsync(content, new JsonSystemSerializer(), cancellationToken);
      var readOperationOption =
         await file.ReadTContentAsync<TestObjectSecond>(new JsonSystemSerializer(), cancellationToken);
      Assert.True(readOperationOption,
         "Read T content failed.");
      if (!readOperationOption.Out(out var reads))
         return;
      Assert.True(reads.Name == content.Name && reads.LastName == content.LastName,
         $"File is empty, {reads} != {content}");
   }
}