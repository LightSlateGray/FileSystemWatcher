# FileSystemWatcher
Simplification of the .NET FileSystemWatcher class

## Description
This project intends to simplify the usage of the [FileSystemWatcher](https://docs.microsoft.com/de-de/dotnet/api/system.io.filesystemwatcher?view=netcore-2.0) class provided by the .NET framework.

### Intended features
* Automatically differentiate between files and directories
* Expose more details about the file/directory being watched
  * Provide the name of the file/directory being watched
  * Support MIME types for files
  * Merge multiple subsequent changes of the same type into one event
  * Provide additional information within the event arguments

## Requirements
* This project is built upon the [.NET Standard 2.0](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md), which is the first version of the API to inclue the FileSystemWatcher class.

## License
This project is licensed under the Apache License 2.0 - see the [LICENSE](./LICENSE) file for details.