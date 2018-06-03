/// <copyright file="FileSystemWatcherFactoryTest.cs">
///   Copyright (c) 2018 LightSlateGray
/// </copyright>
/// <author>
///   LightSlateGray
/// </author>
/// <date>
///   2018-04-27
/// </date>
/// <summary>
///   Unit test fixture for class <see cref="LightSlateGray.FileSystemWatcher.FileSystemWatcherFactory"/>.
/// </summary>
/// <license>
///   Licensed under the Apache License, Version 2.0 (the "License");
///   you may not use this file except in compliance with the License.
///   You may obtain a copy of the License at
///   
///     http://www.apache.org/licenses/LICENSE-2.0
///   
///   Unless required by applicable law or agreed to in writing, software
///   distributed under the License is distributed on an "AS IS" BASIS,
///   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
///   See the License for the specific language governing permissions and
///   limitations under the License.
/// </license>

namespace FileSytemWatcher.Test
{
    using System;
    using System.IO;
    using LightSlateGray.FileSystemWatcher;
    using LightSlateGray.FileSystemWatcher.Enumerations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///   Unit test fixture for class <see cref="FileSystemWatcherFactory"/>.
    /// </summary>
    [TestClass]
    public class FileSystemWatcherFactoryTest
    {
        /// <summary>
        ///   Contains the absolute path to a new temporary file generated for each test case.
        ///   The initialization methods take care of creation and deletion of this file after each test has run.
        /// </summary>
        private string temporaryFileName = null;

        #region Test case initialization methods

        /// <summary>
        ///   Gets called before each test case is run.
        /// </summary>
        [TestInitialize()]
        public void Initialize()
        {
            // Create a new temporary file within the platform's temporary folder and store the absolute path
            this.temporaryFileName = Path.GetTempFileName();
        }

        /// <summary>
        ///   Gets called after the execution of each test case.
        /// </summary>
        [TestCleanup()]
        public void Cleanup()
        {
            // Delete the temporary file within the underlying file system if it exists
            if (File.Exists(this.temporaryFileName))
            {
                File.Delete(this.temporaryFileName);
            }
        }

        #endregion

        /// <summary>
        ///   Verifies that calling <see cref="FileSystemWatcherFactory.CreateInstance(string, IFileSystemWatcherEventHandler)"/>
        ///   supplying a valid path to a file will create a corresponding implementation of the <see cref="IFileSystemWatcher"/> interface.
        /// </summary>
        [TestMethod]
        public void FileSystemWatcherInstanceForFileCreationTest()
        {
            // Retrieve information about the temporary file used within this test case
            var fileInfo = new FileInfo(this.temporaryFileName);

            // Create a new implementation of the IFileSystemWatcher interface for a file
            var fileSystemWatcher = FileSystemWatcherFactory.CreateInstance(fileInfo.FullName);

            // Verify that the implementation of the IFileSystemWatcher interface returns the expected values
            Assert.AreEqual(FileSystemWatcherType.File, fileSystemWatcher.FileSystemWatcherType);
            Assert.IsTrue(fileSystemWatcher.Exists, $"The file {fileInfo.FullName} should exist within the file system.");
            Assert.AreEqual(fileInfo.FullName, fileSystemWatcher.FullPath);
            Assert.AreEqual(fileInfo.Name, fileSystemWatcher.Name);

            // Verify that the implementation of the IFileSystemWatcher interface is not listening for changes by default
            Assert.IsFalse(fileSystemWatcher.IsWatching, $"A new FileSystemWatcher instance should not listen for changes by default.");
        }

        /// <summary>
        ///   Verifies that calling <see cref="FileSystemWatcherFactory.CreateInstance(string, IFileSystemWatcherEventHandler)"/>
        ///   supplying a valid path to a directory will create a corresponding implementation of the <see cref="IFileSystemWatcher"/> interface.
        /// </summary>
        [TestMethod]
        public void FileSystemWatcherInstanceForDirectoryCreationTest()
        {
            // Retrieve information about the directory where the temporary file used within this test case resides in
            var directoryInfo = new FileInfo(this.temporaryFileName).Directory;
            
            // Create a new implementation of the IFileSystemWatcher interface for a directory
            var fileSystemWatcher = FileSystemWatcherFactory.CreateInstance(directoryInfo.FullName);

            // Verify that the implementation of the IFileSystemWatcher interface returns the expected values
            Assert.AreEqual(FileSystemWatcherType.Directory, fileSystemWatcher.FileSystemWatcherType);
            Assert.IsTrue(fileSystemWatcher.Exists, $"The directory {directoryInfo.FullName} should exist within the file system.");
            Assert.AreEqual(directoryInfo.FullName, fileSystemWatcher.FullPath);
            Assert.AreEqual(directoryInfo.Name, fileSystemWatcher.Name);

            // Verify that the implementation of the IFileSystemWatcher interface is not listening for changes by default
            Assert.IsFalse(fileSystemWatcher.IsWatching, $"A new FileSystemWatcher instance should not listen for changes by default.");
        }

        /// <summary>
        ///   Verifies that calling <see cref="FileSystemWatcherFactory.CreateInstance(string, IFileSystemWatcherEventHandler)"/>
        ///   with an invalid path will throw an exception.
        /// </summary>
        /// <param name="path">
        ///   The argument to be supplied to the <see cref="FileSystemWatcherFactory"/> in order to throw an exception.
        /// </param>
        [DataTestMethod]
        [DataRow("")]
        [DataRow(null)]
        [DataRow("This is not a valid path")]
        [DataRow("Ínv@l1d;Cháràçter?")]
        [DataRow("*.*")]
        public void FileSystemWatcherThrowsExceptionForInvalidPath(string path)
        {
            // Passing invalid values as arguments to the factory will throw an exception
            var argumentException = Assert.ThrowsException<ArgumentException>(
                () => FileSystemWatcherFactory.CreateInstance(path),
                $"Calling {nameof(FileSystemWatcherFactory.CreateInstance)} while passing the invalid value '{path}' as argument should throw an exception.");
            Assert.AreEqual(
                "path",
                argumentException.ParamName,
                $"The {nameof(ArgumentException)} should blame parameter 'path' as cause.");
        }

        /// <summary>
        ///   Verifies that calling <see cref="FileSystemWatcherFactory.CreateInstance(string, IFileSystemWatcherEventHandler)"/>
        ///   to watch for a non-existent directory will throw an exception, because the <see cref="FileSystemWatcher"/> instance used
        ///   internally does not support watching non-existent directories.
        /// </summary>
        [TestMethod]
        public void FileSystemWatcherThrowsExceptionForNonExistentPath()
        {
            // Search for a non-existent temporary directory
            var tempDirectory = Path.GetTempPath();
            var directoryName = Path.GetRandomFileName();
            while (Directory.Exists(Path.Combine(tempDirectory, directoryName)))
            {
                directoryName = Path.GetRandomFileName();
            }

            // Retrieve information about the temporary directory used within this test case
            var directoryInfo = new DirectoryInfo(Path.Combine(tempDirectory, directoryName));
            var directoryPath = directoryInfo.FullName + Path.DirectorySeparatorChar;

            // Passing a non-existent path as argument to the factory will throw an exception
            var argumentException = Assert.ThrowsException<ArgumentException>(
                () => FileSystemWatcherFactory.CreateInstance(directoryPath),
                $"Calling {nameof(FileSystemWatcherFactory.CreateInstance)} while passing the invalid value '{directoryPath}' as argument should throw an exception.");
            Assert.AreEqual(
                "path",
                argumentException.ParamName,
                $"The {nameof(ArgumentException)} should blame parameter 'path' as cause.");
        }

        /// <summary>
        ///   Verifies that calling <see cref="FileSystemWatcherFactory.CreateInstance(string, IFileSystemWatcherEventHandler)"/>
        ///   to watch for a file within a non-existent directory will throw an exception, because the <see cref="FileSystemWatcher"/>
        ///   instance used internally does not support watching non-existent directories.
        /// </summary>
        [TestMethod]
        public void FileSystemWatcherThrowsExceptionForFileWithinNonExistentPath()
        {
            // Search for a non-existent temporary directory
            var tempDirectory = Path.GetTempPath();
            var directoryName = Path.GetRandomFileName();
            while (Directory.Exists(Path.Combine(tempDirectory, directoryName)))
            {
                directoryName = Path.GetRandomFileName();
            }

            // Retrieve information about the temporary directory used within this test case
            var directoryInfo = new DirectoryInfo(Path.Combine(tempDirectory, directoryName));
            var fileWithinNonExistantDirectoryPath = Path.Combine(directoryInfo.FullName, Path.GetRandomFileName());

            // Passing a file within a non-existent path as argument to the factory will throw an exception
            var argumentException = Assert.ThrowsException<ArgumentException>(
                () => FileSystemWatcherFactory.CreateInstance(fileWithinNonExistantDirectoryPath),
                $"Calling {nameof(FileSystemWatcherFactory.CreateInstance)} while passing the invalid value '{fileWithinNonExistantDirectoryPath}' as argument should throw an exception.");
            Assert.AreEqual(
                "path",
                argumentException.ParamName,
                $"The {nameof(ArgumentException)} should blame parameter 'path' as cause.");
        }

        /// <summary>
        ///   Verifies that calling <see cref="FileSystemWatcherFactory.CreateInstance(string, IFileSystemWatcherEventHandler)"/>
        ///   supplying a path to a non-existent file will create a valid implementation of the <see cref="IFileSystemWatcher"/> interface.
        /// </summary>
        [TestMethod]
        public void FileSystemWatcherInstanceForNonExistentFileCreationTest()
        {
            // Retrieve information about the temporary file used within this test case
            var fileInfo = new FileInfo(this.temporaryFileName);

            // Delete the file
            fileInfo.Delete();

            // Create a new implementation of the IFileSystemWatcher interface for a non-existent file
            var fileSystemWatcher = FileSystemWatcherFactory.CreateInstance(fileInfo.FullName);

            // Verify that the implementation of the IFileSystemWatcher interface returns the expected values
            Assert.AreEqual(FileSystemWatcherType.File, fileSystemWatcher.FileSystemWatcherType);
            Assert.IsFalse(fileSystemWatcher.Exists, $"The file {fileInfo.FullName} should not exist within the file system.");
            Assert.AreEqual(fileInfo.FullName, fileSystemWatcher.FullPath);
            Assert.AreEqual(fileInfo.Name, fileSystemWatcher.Name);

            // Verify that the implementation of the IFileSystemWatcher interface is not listening for changes by default
            Assert.IsFalse(fileSystemWatcher.IsWatching, $"A new FileSystemWatcher instance should not listen for changes by default.");
        }
    }
}
