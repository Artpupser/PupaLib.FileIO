using System;
using System.Threading.Tasks;
using PupaLib.FileIO.Example.Extensions;
using PupaLib.FileIO.Example.Objects;
using PupaLib.FileIO.Serializers;

namespace PupaLib.FileIO.Example;

public static class ExampleApp {
  public static async Task Main(string[] args) {
    Console.WriteLine("Hello PupaLib.FileIO.Example!");
    await Example1();
    await Example2();
    await Example3_1();
    await Example3_2();
    await Example4();
    await Example5();
    await Example6();
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey(true);
    await Task.CompletedTask;
  }

  //Creation file
  public static async Task Example1() {
    var fileName = "file.txt";
    var file = VirtualIo.RootFolder.CreateFileIn(fileName); // Create file with name file.txt
    Console.WriteLine(file.GetInfo());
    await Task.CompletedTask;
  }

  //Load file if exists 
  public static async Task Example2() {
    var fileName = "file.txt";
    var file = VirtualIo.RootFolder.GetFileIn(fileName); // Gets file with name file.txt 
    if(file is not null) {
      //Check file on nullable
      Console.WriteLine(file.GetInfo());
    }

    await Task.CompletedTask;
  }

  //Write and read content in file
  public static async Task Example3_1() {
    var fileName = "file.txt";
    var file = VirtualIo.RootFolder.GetFileIn(fileName);
    if(file is not null) {
      var content = "Hello world!";
      await file.WriteStringAsync(content); // Write file
      Console.WriteLine($"Writes content [{content}] in file");
      Console.WriteLine($"File content: {await file.ReadStringAsync()}"); //Read file
      Console.WriteLine(file.GetInfo());
    }

    await Task.CompletedTask;
  }

  public static async Task Example3_2() {
    var fileName = "file.txt";
    var file = VirtualIo.RootFolder.GetFileIn(fileName);
    if(file is not null) {
      var content = new ExampleObject("Name", "Lastname", [12, 53, 47]);
      file.WriteTContent<ExampleObject>(content, new JsonSystemSerializer()); // Write and serialize file
      Console.WriteLine($"Writes content [\n{content}\n] in file");
      Console.WriteLine(
        $"File content: {file.ReadTContent<ExampleObject>(new JsonSystemSerializer())}"); //Read and deserialize file
      Console.WriteLine(file.GetInfo());
    }

    await Task.CompletedTask;
  }

  //Deleting file
  public static async Task Example4() {
    var fileName = "file.txt";
    var file = VirtualIo.RootFolder.GetFileIn(fileName);
    if(file is not null) {
      file.DeleteMe(); // Delete file
      Console.WriteLine($"File deleted, exists: {file.Exists}");
    }

    await Task.CompletedTask;
  }

  //Creating folder in root folder
  public static async Task Example5() {
    var folderName = "folder";
    var folder = VirtualIo.RootFolder.CreateFolderIn(folderName);
    Console.WriteLine(folder.GetInfo());
    await Task.CompletedTask;
  }

  //Delete folder
  public static async Task Example6() {
    var folderName = "folder";
    var folder = VirtualIo.RootFolder.GetFolderIn(folderName);
    if(folder is not null) {
      folder.DeleteMe(); //Delete folder
      Console.WriteLine($"Folder deleted, exists: {folder.Exists}");
    }

    await Task.CompletedTask;
  }
}