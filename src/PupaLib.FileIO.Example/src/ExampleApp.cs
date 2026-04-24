using System;
using System.Threading;
using System.Threading.Tasks;

using PupaLib.FileIO.Example.Extensions;
using PupaLib.FileIO.Example.Objects;
using PupaLib.FileIO.Serializers;

namespace PupaLib.FileIO.Example;

/// <summary>
/// Demonstrates PupaLib.FileIO library usage.
/// </summary>
public static class ExampleApp {
   /// <summary>
   /// Sample filename used in examples: "file.txt".
   /// </summary>
   private const string ExampleFileName = "file.txt";

   /// <summary>
   /// Sample folder name used in examples: "folder".
   /// </summary>
   private const string ExampleFolderName = "folder";

   /// <summary>
   /// Runs all FileIO examples sequentially.
   /// </summary>
   public static async Task Main() {
      var cts = new CancellationTokenSource();
      cts.CancelAfter(TimeSpan.FromSeconds(10));
      Console.WriteLine("Hello PupaLib.FileIO.Example!");
      Example1();
      await Example2(cts.Token);
      await Example3_1(cts.Token);
      await Example3_2(cts.Token);
      Example4();
      Example5();
      Example6();
   }

   /// <summary>
   /// Creates a file if it doesn't exist and shows file info.
   /// </summary>
   public static void Example1(CancellationToken cancellationToken = default) {
      var fileOption = VirtualIo.RootFolder.GetOrCreateFileIn(ExampleFileName);
      if (!fileOption.Out(out var file))
         return;
      Console.Write($"{file.GetInfo()}\n");
   }

   /// <summary>
   /// Loads existing file or shows null if not found.
   /// </summary>
   public static async Task Example2(CancellationToken cancellationToken = default) {
      var fileOption = VirtualIo.RootFolder.GetFileIn(ExampleFileName);
      if (fileOption.Out(out var file))
         Console.Write($"{file.GetInfo()}\n");
      await Task.CompletedTask;
   }

   /// <summary>
   /// Writes and reads plain text content to/from file.
   /// </summary>
   public static async Task Example3_1(CancellationToken cancellationToken = default) {
      var fileOption = VirtualIo.RootFolder.GetFileIn(ExampleFileName);
      if (fileOption.Out(out var file)) {
         const string content = "Hello world!";
         await file.WriteStringAsync(content, cancellationToken);
         Console.Write(
            $"Written: [{content}]\nRead: {await file.ReadStringAsync(cancellationToken)}\n{file.GetInfo()}\n");
      }

      await Task.CompletedTask;
   }

   /// <summary>
   /// Serializes and deserializes object to/from JSON file.
   /// </summary>
   public static async Task Example3_2(CancellationToken cancellationToken = default) {
      var fileOption = VirtualIo.RootFolder.GetFileIn(ExampleFileName);
      if (fileOption.Out(out var file)) {
         var content = new ExampleObject("Name", "Lastname", [12, 53, 47]);
         await file.WriteTContentAsync(content, new JsonSystemSerializer(), cancellationToken);
         Console.Write(
            $"Written object:\n{content}\nRead object:{file.ReadTContent<ExampleObject>(new JsonSystemSerializer())}\n{file.GetInfo()}");
      }

      await Task.CompletedTask;
   }

   /// <summary>
   /// Deletes the example file.
   /// </summary>
   public static void Example4() {
      var fileOption = VirtualIo.RootFolder.GetFileIn(ExampleFileName);
      if (!fileOption.Out(out var file))
         return;
      file.DeleteMe();
      Console.Write($"File deleted, exists: {file.Exists}\n");
   }

   /// <summary>
   /// Creates example folder if it doesn't exist.
   /// </summary>
   public static void Example5() {
      var folderOption = VirtualIo.RootFolder.GetOrCreateFolderIn(ExampleFolderName);
      if (!folderOption.Out(out var folder))
         return;
      Console.Write($"{folder.GetInfo()}\n");
   }

   /// <summary>
   /// Deletes the example folder.
   /// </summary>
   public static void Example6() {
      var folderOption = VirtualIo.RootFolder.GetFolderIn(ExampleFolderName);
      if (!folderOption.Out(out var folder))
         return;
      folder.DeleteMe();
      Console.Write($"Folder deleted, exists: {folder.Exists}\n");
   }
}