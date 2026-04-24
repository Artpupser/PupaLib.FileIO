<div align="center">

# PupaLib.FileIO

![C#](https://img.shields.io/badge/C%23-black.svg?style=for-the-badge&logo=csharp&logoColor=white)
![Dotnet](https://img.shields.io/badge/.NET-black?style=for-the-badge&logo=dotnet&logoColor=white)
![Nuget](https://img.shields.io/badge/NuGet-black?style=for-the-badge&logo=nuget&logoColor=white)
![Github]( https://img.shields.io/badge/GitHub-black?style=for-the-badge&logo=github&logoColor=white)
![License](https://img.shields.io/badge/MIT-black?style=for-the-badge)

![NuGet](https://img.shields.io/nuget/v/PupaLib.FileIO.svg?style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-10.0-blue?style=for-the-badge)

#### [PupaLib.FileIO](https://www.nuget.org/packages/PupaLib.FileIO) is an incredibly simple library for working with files and folders. 🎯

<img src="https://github.com/Artpupser/PupaLib.FileIO/blob/main/assets/banner.jpg" style="border-radius: 20px; max-height: 500px">

</div>

## 📜 Content

- [🗝️ Key Features](#-key-features)
- [🚀️ Installation](#-installation)
- [💡 Usage](#-usage)
- [🗃️ Devlog](#-devlog)
- [⚖️️ License](#-license)


## 🗝️ Key Features

<div align="center">

| 🏆 Feature                  | 📝 Description                                                            |
|-----------------------------|---------------------------------------------------------------------------|
| **Easy to Use**             | Simplifies file manipulation tasks 🛠️                                    |
| **Versatile Functionality** | - Create new files and folders 📂<br>- Read and write data efficiently 📄 |
| **Cross-Platform Support**  | Works seamlessly across different operating systems 🌍                    |
| **Lightweight**             | Minimal impact on performance ⚡                                           |

</div>

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

Here is a practical guide on how to use the `VirtualFile`, `VirtualFolder`, and `VirtualIo` classes to manage your file
system operations efficiently.

### 🛠️ File and Folder Creation

**Creating a folder:**

```csharp
var path = "./data/new_folder";
var folder = await VirtualFolder.GetOrCreateFolder(path); // Returns Option<VirtualFolder>

if (folder.Out(out var _folder))
{
    Console.WriteLine($"Folder created: {_folder.MyPath}");
}
```

**Creating a file:**

```csharp
var filePath = "./data/test.txt";
var file = await VirtualFile.GetOrCreate(filePath); // Returns Option<VirtualFile>

if (file.Out(out var _file))
{
    Console.WriteLine($"File created: {_file.MyPath}");
}
```

> **Note:** We use `GetOrCreateFolder` / `GetOrCreate` which handles both existing resources and creates new ones if
> they don't exist. The example above assumes you await the option and unwrap the successful result. If you want to use
`Option<VirtualFolder>`, you can simply check `if (folder) {}`.

### 🔎 Check If File or Folder Exists

**Check for a file:**

```csharp
var filePath = "./path/file_name.txt";
var file = VirtualFile.GetFile(filePath);

if (file.Out(out var _file))
{
    Console.WriteLine($"File exists, path -> {_file.MyPath}");
}
else
{
    Console.WriteLine("File does not exist");
}
```

**Check for a folder:**

```csharp
var folderPath = "./path/folder_name";
var folder = VirtualFolder.GetFolder(folderPath);

if (folder.Out(out var _folder))
{
    Console.WriteLine($"Folder exists, path -> {_folder.MyPath}");
}
else
{
    Console.WriteLine("Folder does not exist");
}
```

### 📝 Write and Read Files

**Writing to a text file:**

```csharp
var fileName = "message.txt";
var file = VirtualIo.RootFolder.GetFileIn(fileName);

if (file.Out(out var _file)) // Or use GetOrCreate if you want to create it if missing
{
    // Or use GetOrCreate if the file might not exist:
    // var file = VirtualFile.GetOrCreate(fileName).Content.Value; 

    var content = "Hello world!";
    await _file.WriteStringAsync(content); // Write file
    
    Console.WriteLine($"Writes content [{content}] in file");
    
    var readContent = await _file.ReadStringAsync(); // Read file
    Console.WriteLine($"File content: {readContent}");
    
    Console.WriteLine($"File size: {_file.SizeInBytes} bytes");
    Console.WriteLine($"Last modified: {_file.LastWriteTime}");
}
```

**Writing with serialization:**

```csharp
var fileName = "user.json";
var file = VirtualIo.RootFolder.GetFileIn(fileName);

if (file.Out(out var _file))
{
    var content = new { name = "Alice", age = 30 }; // Anonymous type or custom class
    await _file.WriteTContentAsync(content, new JsonSystemSerializer()); // Write and serialize
    
    Console.WriteLine($"Writes content [\n{content}\n] in file");
    
    var readUser = await _file.ReadTContentAsync<Dictionary<string, object>>(new JsonSystemSerializer()); 
    // Note: Use specific type for ReadTContent, e.g., MyUserClass
    // var readUser = await _file.ReadTContentAsync<MyUser>(new JsonSystemSerializer());
    
    Console.WriteLine($"Read and deserialized content: {readUser}");
}
```

**Alternative using `GetOrCreate` (Creates file if missing):**

```csharp
// Creates file if it doesn't exist, then writes content
var file = VirtualIo.RootFolder.GetOrCreateFileIn("data/data.bin").Content.Value; 
file.WriteBytes(new byte[] { 1, 2, 3 });
```

### 🗑️ Delete File and Folder

**Deleting a file:**

```csharp
var fileName = "temp.txt";
var file = VirtualIo.RootFolder.GetFileIn(fileName);

if (file.Out(out var _file))
{
    _file.DeleteMe(); // Delete file
    Console.WriteLine($"File deleted, exists: {_file.Exists}"); // false
}
```

**Deleting a folder (with recursive option):**

```csharp
var folderName = "old_data";
var folder = VirtualIo.RootFolder.GetFolderIn(folderName);

if (folder.Out(out var _folder))
{
    _folder.DeleteMe(full: true); // full = true deletes recursively
    Console.WriteLine($"Folder deleted, exists: {_folder.Exists}");
}
```

### 📊 Getting File/Folder Information

You can easily access metadata properties without writing code:

```csharp
var file = VirtualIo.RootFolder.GetFileIn("test.txt");
if (file.Out(out var _file))
{
    Console.WriteLine($"Name: {_file.Name}");
    Console.WriteLine($"Extension: {_file.Extension}");
    Console.WriteLine($"Creation Time: {_file.CreationTime}");
    Console.WriteLine($"Size in Bytes: {_file.SizeInBytes}");
}

// For folders
var folder = VirtualIo.RootFolder.GetFolder("my_project");
if (folder.Out(out var _folder))
{
    Console.WriteLine($"Folder Name: {_folder.Name}");
    Console.WriteLine($"Total Files Count: {_folder.GetFilesCount()}");
    Console.WriteLine($"Total Folders Count: {_folder.GetDirectoriesCount()}");
}
```

### 🚀 Enumerating Contents

```csharp
var rootFiles = VirtualIo.RootFolder.EnumerateFiles(SearchOption.AllDirectories);
foreach (var vFile in rootFiles)
{
    Console.WriteLine($"Found file: {vFile.MyPath}");
}

var subFolders = VirtualIo.RootFolder.EnumerateFolders(SearchOption.AllDirectories);
foreach (var vFolder in subFolders)
{
    Console.WriteLine($"Found folder: {vFolder.MyPath}");
}
```

## 🗃️ Devlog

### 1.3.0

* **Added:** `RestorePackagesWithLockFile` field in `PropertyGroup` of `.csproj` files.
* **Added:** `PupaLib.Core` dependency.
* **Changes:** Introduced a new error handling system based on `Option<T>` (from `PupaLib.Core`) across the entire
  library.
* **Rewrite:** Major refactoring to ensure `VirtualFolder` and `VirtualFile` methods return `Option<T>` results instead
  of `null` or throwing raw exceptions for missing files/folders.

### 1.2.1

- Changed .NET SDK version, .net8.0 -> .net10.0

## ⚖️ License

This project is licensed under the MIT License.
