/// <copyright file="FileSystemWatcherEventHandlerTest.cs">
///   Copyright (c) 2018 LightSlateGray
/// </copyright>
/// <author>
///   LightSlateGray
/// </author>
/// <date>
///   2018-04-28
/// </date>
/// <summary>
///   Unit test fixture for interface <see cref="LightSlateGray.FileSystemWatcher.Interfaces.IFileWatcherEventHandler"/>.
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

namespace FileSystemWatcher.Test
{
    using System;
    using System.IO;
    using System.Threading;
    using LightSlateGray.FileSystemWatcher;
    using LightSlateGray.FileSystemWatcher.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    ///   Unit test fixture for interface <see cref="LightSlateGray.FileSystemWatcher.Interfaces.IFileWatcherEventHandler"/>.
    /// </summary>
    [TestClass]
    public class FileSystemWatcherEventHandlerTest
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
        ///   Verifies that the creation of currently non-existent file will invoke the corresponding event handler callback method
        ///   while the implementation of the <see cref="IFileSystemWatcher"/> instance is actively watching.
        /// </summary>
        [TestMethod]
        public void FileCreationWillInvokeEventHandlerTest()
        {
            // Delete the temporary file of this test case
            var fileInfo = new FileInfo(this.temporaryFileName);
            fileInfo.Delete();

            // Create a new implementation of the IFileSystemWatcher interface for a non-existent file
            var fileSystemWatcher = FileSystemWatcherFactory.CreateInstance(fileInfo.FullName);

            // Verify that the file does not exist within the file system
            Assert.IsFalse(fileInfo.Exists, $"The file {fileInfo.FullName} should not exists within the file system.");
            Assert.IsFalse(fileSystemWatcher.Exists, $"The file {fileInfo.FullName} should not exists within the file system.");

            // Helper variables to verify the expectations
            var eventChangeType = WatcherChangeTypes.All;
            var eventFileName = string.Empty;
            var eventFullPath = string.Empty;
            var eventSender = default(object);

            // Create an instance of the EventWaitHandle class to wait for the callback to be invoked
            var eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            // Create a mocked implementation of the IFileSystemWatcherEventHandler interface
            var eventHandlerMock = new Mock<IFileSystemWatcherEventHandler>();
            eventHandlerMock
                .Setup(mock => mock.OnCreated(It.IsAny<IFileSystemWatcher>(), It.IsAny<FileSystemEventArgs>()))
                .Callback<object, FileSystemEventArgs>((sender, eventArgs) =>
                {
                    // Update the helper variables
                    eventChangeType = eventArgs.ChangeType;
                    eventFileName = eventArgs.Name;
                    eventFullPath = eventArgs.FullPath;
                    eventSender = sender;

                    // Set the event handle to notify the waiting thread that the event callback method has been handled
                    eventWaitHandle.Set();
                });

            // Wire up the mocked event handler
            fileSystemWatcher.FileSystemWatcherEventHandler = eventHandlerMock.Object;

            // Start watching for changes
            fileSystemWatcher.StartWatching();

            // Verify that the implementation of the IFileSystemWatcher interface is watching for changes
            Assert.IsTrue(fileSystemWatcher.IsWatching, $"The {nameof(fileSystemWatcher)} should actively watch for changes when started.");

            // Create the file within the underlying file system and dispose the stream immediately
            fileInfo.Create().Dispose();

            // Wait for the event handler callback method to be invoked, but at most for five seconds
            eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5.0d));

            // Verify that the file does exist within the file system
            Assert.IsTrue(fileInfo.Exists, $"The newly created file {fileInfo.FullName} should exists within the file system.");
            Assert.IsTrue(fileSystemWatcher.Exists, $"The newly created file {fileInfo.FullName} should exists within the file system.");

            // Verify that the expected arguments have been supplied to callback method of the event handler
            Assert.AreSame(fileSystemWatcher, eventSender, $"The {nameof(fileSystemWatcher)} should be the sender of the event.");
            Assert.AreEqual(eventChangeType, WatcherChangeTypes.Created, $"The event handler callback should be called for change type {WatcherChangeTypes.Created}.");
            Assert.AreEqual(eventFileName, fileSystemWatcher.Name, $"The event handler callback should be called for file {fileSystemWatcher.Name}.");
            Assert.AreEqual(eventFullPath, fileSystemWatcher.FullPath, $"The event handler callback should be called for a file within {fileSystemWatcher.FullPath}.");
        }
        
        /// <summary>
        ///   Verifies that the deletion of an existing file will invoke the corresponding event handler callback method
        ///   while the implementation of the <see cref="IFileSystemWatcher"/> instance is actively watching.
        /// </summary>
        [TestMethod]
        public void FileDeletionWillInvokeEventHandlerTest()
        {
            // Retrieve information about the temporary file of this test case
            var fileInfo = new FileInfo(this.temporaryFileName);

            // Create a new implementation of the IFileSystemWatcher interface for an existing file
            var fileSystemWatcher = FileSystemWatcherFactory.CreateInstance(fileInfo.FullName);

            // Verify that the file does exist within the file system
            Assert.IsTrue(fileInfo.Exists, $"The file {fileInfo.FullName} should exists within the file system.");
            Assert.IsTrue(fileSystemWatcher.Exists, $"The file {fileInfo.FullName} should exists within the file system.");

            // Helper variables to verify the expectations
            var eventChangeType = WatcherChangeTypes.All;
            var eventFileName = string.Empty;
            var eventFullPath = string.Empty;
            var eventSender = default(object);

            // Create an instance of the EventWaitHandle class to wait for the callback to be invoked
            var eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            // Create a mocked implementation of the IFileSystemWatcherEventHandler interface
            var eventHandlerMock = new Mock<IFileSystemWatcherEventHandler>();
            eventHandlerMock
                .Setup(mock => mock.OnDeleted(It.IsAny<IFileSystemWatcher>(), It.IsAny<FileSystemEventArgs>()))
                .Callback<object, FileSystemEventArgs>((sender, eventArgs) =>
                {
                    // Update the helper variables
                    eventChangeType = eventArgs.ChangeType;
                    eventFileName = eventArgs.Name;
                    eventFullPath = eventArgs.FullPath;
                    eventSender = sender;

                    // Set the event handle to notify the waiting thread that the event callback method has been handled
                    eventWaitHandle.Set();
                });

            // Wire up the mocked event handler
            fileSystemWatcher.FileSystemWatcherEventHandler = eventHandlerMock.Object;

            // Start watching for changes
            fileSystemWatcher.StartWatching();

            // Verify that the implementation of the IFileSystemWatcher interface is watching for changes
            Assert.IsTrue(fileSystemWatcher.IsWatching, $"The {nameof(fileSystemWatcher)} should actively watch for changes when started.");

            // Delete the temporary file of this test case
            fileInfo.Delete();

            // Wait for the event handler callback method to be invoked, but at most for five seconds
            eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5.0d));

            // Verify that the file does no longer exist within the file system
            Assert.IsFalse(fileInfo.Exists, $"The deleted file {fileInfo.FullName} should no longer exist within the file system.");
            Assert.IsFalse(fileSystemWatcher.Exists, $"The deleted file {fileInfo.FullName} should no longer exist within the file system.");

            // Verify that the expected arguments have been supplied to callback method of the event handler
            Assert.AreSame(fileSystemWatcher, eventSender, $"The {nameof(fileSystemWatcher)} should be the sender of the event.");
            Assert.AreEqual(eventChangeType, WatcherChangeTypes.Deleted, $"The event handler callback should be called for change type {WatcherChangeTypes.Deleted}.");
            Assert.AreEqual(eventFileName, fileSystemWatcher.Name, $"The event handler callback should be called for file {fileSystemWatcher.Name}.");
            Assert.AreEqual(eventFullPath, fileSystemWatcher.FullPath, $"The event handler callback should be called for a file within {fileSystemWatcher.FullPath}.");
        }

        /// <summary>
        ///   Verifies that changes to an existing file will invoke the corresponding event handler callback method
        ///   while the implementation of the <see cref="IFileSystemWatcher"/> instance is actively watching.
        /// </summary>
        [TestMethod]
        public void FileChangeWillInvokeEventHandlerTest()
        {
            // Retrieve information about the temporary file of this test case
            var fileInfo = new FileInfo(this.temporaryFileName);

            // Create a new implementation of the IFileSystemWatcher interface for an existing file
            var fileSystemWatcher = FileSystemWatcherFactory.CreateInstance(fileInfo.FullName);

            // Verify that the file does exist within the file system
            Assert.IsTrue(fileInfo.Exists, $"The file {fileInfo.FullName} should exists within the file system.");
            Assert.IsTrue(fileSystemWatcher.Exists, $"The file {fileInfo.FullName} should exists within the file system.");

            // Helper variables to verify the expectations
            var eventChangeType = WatcherChangeTypes.All;
            var eventFileName = string.Empty;
            var eventFullPath = string.Empty;
            var eventSender = default(object);

            // Create an instance of the EventWaitHandle class to wait for the callback to be invoked
            var eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            // Create a mocked implementation of the IFileSystemWatcherEventHandler interface
            var eventHandlerMock = new Mock<IFileSystemWatcherEventHandler>();
            eventHandlerMock
                .Setup(mock => mock.OnChanged(It.IsAny<IFileSystemWatcher>(), It.IsAny<FileSystemEventArgs>()))
                .Callback<object, FileSystemEventArgs>((sender, eventArgs) =>
                {
                    // Update the helper variables
                    eventChangeType = eventArgs.ChangeType;
                    eventFileName = eventArgs.Name;
                    eventFullPath = eventArgs.FullPath;
                    eventSender = sender;

                    // Set the event handle to notify the waiting thread that the event callback method has been handled
                    eventWaitHandle.Set();
                });

            // Wire up the mocked event handler
            fileSystemWatcher.FileSystemWatcherEventHandler = eventHandlerMock.Object;

            // Start watching for changes
            fileSystemWatcher.StartWatching();

            // Verify that the implementation of the IFileSystemWatcher interface is watching for changes
            Assert.IsTrue(fileSystemWatcher.IsWatching, $"The {nameof(fileSystemWatcher)} should actively watch for changes when started.");

            // Update the contents of the temporary file
            using (var stream = fileInfo.AppendText())
            {
                stream.WriteLine($"Append text content to file {fileInfo.Name}.");
                stream.Flush();
            }

            // Wait for the event handler callback method to be invoked, but at most for five seconds
            eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5.0d));

            // Verify that the expected arguments have been supplied to callback method of the event handler
            Assert.AreSame(fileSystemWatcher, eventSender, $"The {nameof(fileSystemWatcher)} should be the sender of the event.");
            Assert.AreEqual(eventChangeType, WatcherChangeTypes.Changed, $"The event handler callback should be called for change type {WatcherChangeTypes.Changed}.");
            Assert.AreEqual(eventFileName, fileSystemWatcher.Name, $"The event handler callback should be called for file {fileSystemWatcher.Name}.");
            Assert.AreEqual(eventFullPath, fileSystemWatcher.FullPath, $"The event handler callback should be called for a file within {fileSystemWatcher.FullPath}.");
        }
        
        /// <summary>
        ///   Verifies that changing the file name of an existing file will invoke the corresponding event handler callback method
        ///   while the implementation of the <see cref="IFileSystemWatcher"/> instance is actively watching.
        /// </summary>
        [TestMethod]
        public void FileRenamingWillInvokeEventHandlerTest()
        {
            // Retrieve information about the temporary file of this test case
            var fileInfo = new FileInfo(this.temporaryFileName);

            // Create a new implementation of the IFileSystemWatcher interface for an existing file
            var fileSystemWatcher = FileSystemWatcherFactory.CreateInstance(fileInfo.FullName);

            // Verify that the file does exist within the file system
            Assert.IsTrue(fileInfo.Exists, $"The file {fileInfo.FullName} should exists within the file system.");
            Assert.IsTrue(fileSystemWatcher.Exists, $"The file {fileInfo.FullName} should exists within the file system.");

            // Helper variables to verify the expectations
            var eventChangeType = WatcherChangeTypes.All;
            var eventFileName = string.Empty;
            var eventFullPath = string.Empty;
            var eventOldFileName = string.Empty;
            var eventOldFullPath = string.Empty;
            var eventSender = default(object);

            // Create an instance of the EventWaitHandle class to wait for the callback to be invoked
            var eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            // Create a mocked implementation of the IFileSystemWatcherEventHandler interface
            var eventHandlerMock = new Mock<IFileSystemWatcherEventHandler>();
            eventHandlerMock
                .Setup(mock => mock.OnRenamed(It.IsAny<IFileSystemWatcher>(), It.IsAny<RenamedEventArgs>()))
                .Callback<object, RenamedEventArgs>((sender, eventArgs) =>
                {
                    // Update the helper variables
                    eventChangeType = eventArgs.ChangeType;
                    eventFileName = eventArgs.Name;
                    eventFullPath = eventArgs.FullPath;
                    eventOldFileName = eventArgs.OldName;
                    eventOldFullPath = eventArgs.OldFullPath;
                    eventSender = sender;

                    // Set the event handle to notify the waiting thread that the event callback method has been handled
                    eventWaitHandle.Set();
                });

            // Wire up the mocked event handler
            fileSystemWatcher.FileSystemWatcherEventHandler = eventHandlerMock.Object;

            // Start watching for changes
            fileSystemWatcher.StartWatching();

            // Verify that the implementation of the IFileSystemWatcher interface is watching for changes
            Assert.IsTrue(fileSystemWatcher.IsWatching, $"The {nameof(fileSystemWatcher)} should actively watch for changes when started.");

            // Randomly generate a new file name to which the file is being renamed
            var newFilePath = Path.Combine(fileInfo.Directory.FullName, Path.GetRandomFileName());
            while (File.Exists(newFilePath))
            {
                newFilePath = Path.Combine(fileInfo.Directory.FullName, Path.GetRandomFileName());
            }

            // Move the temporary file to a new path, which effectively renames the file and updates itself accordingly
            fileInfo.MoveTo(newFilePath);

            // Wait for the event handler callback method to be invoked, but at most for five seconds
            eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5.0d));

            // Create an instance of the FileInfo class for the old file
            var oldFileInfo = new FileInfo(this.temporaryFileName);

            // Verify that the old file does no longer exist within the file system
            Assert.IsFalse(oldFileInfo.Exists, $"The old file {oldFileInfo.FullName} should no longer exist within the file system.");
            Assert.IsFalse(fileSystemWatcher.Exists, $"The old file {oldFileInfo.FullName} should no longer exist within the file system.");

            // Verify that the new file does exist within the file system
            Assert.IsTrue(fileInfo.Exists, $"The file {fileInfo.FullName} should exists within the file system.");

            // Verify that the expected arguments have been supplied to callback method of the event handler
            Assert.AreSame(fileSystemWatcher, eventSender, $"The {nameof(fileSystemWatcher)} should be the sender of the event.");
            Assert.AreEqual(eventChangeType, WatcherChangeTypes.Renamed, $"The event handler callback should be called for change type {WatcherChangeTypes.Renamed}.");
            Assert.AreEqual(eventOldFileName, fileSystemWatcher.Name, $"The event handler callback should be called for file {fileSystemWatcher.Name}.");
            Assert.AreEqual(eventOldFullPath, fileSystemWatcher.FullPath, $"The event handler callback should be called for a file within {fileSystemWatcher.FullPath}.");
        }
    }

}
