# 🧪️ PupaLib.FileIO
![C#](https://img.shields.io/badge/CSHARP-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![Dotnet](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Nuget](https://img.shields.io/badge/NuGet-004880?style=for-the-badge&logo=nuget&logoColor=white)
![Github]( https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white)
![License](https://img.shields.io/badge/MIT-black?style=for-the-badge)

![NuGet](https://img.shields.io/nuget/v/PupaLib.FileIO.svg)

#### PupaLib.FileIO is an incredibly simple library for working with files and folders. 🎯

---

## 🗝️ Key Features
| 🏆 Feature              | 📝 Description                                         |
|------------------------|-----------------------------------------------------|
| **Easy to Use**        | Simplifies file manipulation tasks 🛠️               |
| **Versatile Functionality** | - Create new files and folders 📂<br>- Read and write data efficiently 📄 |
| **Cross-Platform Support** | Works seamlessly across different operating systems 🌍 |
| **Lightweight**        | Minimal impact on performance ⚡        |

## 🚀 Installation

You can install the package through the NuGet Package Manager or via the command line

```bash
dotnet add package PupaLib.FileIO
```

Or through the NuGet Package Manager

```bash
Install-Package PupaLib.FileIO
```

## 💡 Usage
### 🛠️ File or Folder Creation

Create a file:

```csharp
var path = "./path/file_name.txt";
var file = VirtualFile.CreateFile(path);
```
Create a folder:
```csharp
var path = "./path/folder_name";
var folder = VirtualFile.CreateFolder(path);
```

### 🔎 Check If File or Folder Exists

Check for a file:

```csharp
var path = "./path/file_name.txt";
var file = VirtualFile.GetFile(path);
if(file is not null) {
    Console.WriteLine($"File exists, path -> {file.MyPath}");    
} else {
    Console.WriteLine($"File not exists")
}
```
Check for a folder:
```csharp
var path = "./path/folder_name";
var folder = VirtualFolder.GetFolder(path);
if(folder is not null) {
    Console.WriteLine($"Folder exists, path -> {folder.MyPath}");    
} else {
    Console.WriteLine($"Folder not exists")
}
```
### 📝 Write and Read Files

Writing to a text file:

```csharp
var fileName = "file.txt";
var file = VirtualIo.RootFolder.GetFileIn(fileName);
if(file is not null) {
  var content = "Hello world!";
  await file.WriteStringAsync(content); // Write file
  Console.WriteLine($"Writes content [{content}] in file");
  Console.WriteLine($"File content: {await file.ReadStringAsync()}"); //Read file
  Console.WriteLine(file.GetInfo());
}
```

Writing with serialization:

```csharp
var fileName = "file.txt";
var file = VirtualIo.RootFolder.GetFileIn(fileName);
if(file is not null) {
  var content = new ExampleObject("Name", "Lastname", [12, 53, 47]);
  file.WriteTContent<ExampleObject>(content, new JsonSystemSerializer()); // Write and serialize file
  Console.WriteLine($"Writes content [\n{content}\n] in file");
  Console.WriteLine($"File content: {file.ReadTContent<ExampleObject>(new JsonSystemSerializer())}"); //Read and deserialize file
  Console.WriteLine(file.GetInfo());
}
```
### 🗑️ Delete File and Folder

Deleting a file:

```csharp
var fileName = "file.txt";
var file = VirtualIo.RootFolder.GetFileIn(fileName);
if(file is not null) {
  file.DeleteMe(); // Delete file
  Console.WriteLine($"File deleted, exists: {file.Exists}");
}
```

Deleting a folder:

```csharp
var folderName = "folder";
var folder = VirtualIo.RootFolder.GetFolderIn(folderName);
if(folder is not null) {
  folder.DeleteMe(); //Delete folder
  Console.WriteLine($"Folder deleted, exists: {folder.Exists}");
}
```
