# FileSystemWatcher
Simplification of the [.NET FileSystemWatcher](https://docs.microsoft.com/de-de/dotnet/api/system.io.filesystemwatcher?view=netstandard-2.0) class

[![Build Status | branch development](https://travis-ci.org/LightSlateGray/FileSystemWatcher.svg?branch=development)](https://travis-ci.org/LightSlateGray/FileSystemWatcher)

## Description
This project intends to simplify the usage of the
  [FileSystemWatcher](https://docs.microsoft.com/de-de/dotnet/api/system.io.filesystemwatcher?view=netstandard-2.0)
  class defined by the .NET Standard 2.0.

### Intended features
* [x] Automatically differentiate between files and directories using the utility classes
      [File](https://docs.microsoft.com/de-de/dotnet/api/system.io.file?view=netstandard-2.0) and
      [Path](https://docs.microsoft.com/de-de/dotnet/api/system.io.path?view=netstandard-2.0)
* [ ] Expose more details about the file/directory being watched:
  * [x] Provide the name of the file/directory being watched on each event
  * [x] Unify the delegate signature invoked by the event handler
  * [ ] Merge multiple subsequent changes of the same type into one event (see issue #4)

## Requirements
* This project is built upon the
  [.NET Standard 2.0](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md),
  which is the first version of the API to include the FileSystemWatcher class.

## License
This project is licensed under the Apache License 2.0 - see the [LICENSE](./LICENSE) file for details.
