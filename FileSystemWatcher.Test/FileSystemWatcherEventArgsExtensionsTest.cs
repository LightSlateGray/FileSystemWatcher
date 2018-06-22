/// <copyright file="FileSystemEventArgsExtensionsTest.cs">
///   Copyright (c) 2018 LightSlateGray
/// </copyright>
/// <author>
///   LightSlateGray
/// </author>
/// <date>
///   2018-06-04
/// </date>
/// <summary>
///   Unit test fixture for extension methods defined in
///   <see cref="LightSlateGray.FileSystemWatcher.Extensions.FileSystemEventArgsExtensions"/>.
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

namespace LightSlateGray.FileSystemWatcher.Test
{
    using System.IO;
    using LightSlateGray.FileSystemWatcher.Enumerations;
    using LightSlateGray.FileSystemWatcher.Events;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using LightSlateGray.FileSystemWatcher.Extensions;
    using System;

    /// <summary>
    ///   Unit test fixture for extension methods defined in <see cref="FileSystemEventArgsExtensions"/>.
    /// </summary>
    [TestClass]
    public class FileSystemEventArgsExtensionsTest
    {
        /// <summary>
        ///   Verifies that all values will be preserved during the conversion of an instance of class
        ///   <see cref="FileSystemEventArgs"/> to an instance of class <see cref="FileSystemWatcherEventArgs"/>
        ///   and that the <see cref="FileSystemWatcherEventType"/> of the converted instance matches the expectations.
        /// </summary>
        /// <param name="changeType">
        ///   A value of the <see cref="WatcherChangeTypes"/> enumeration set for the instance of the
        ///   <see cref="FileSystemEventArgs"/> instance to be converted.
        /// </param>
        /// <param name="expectedEventType">
        ///   The expected value of the <see cref="FileSystemWatcherEventType"/> of the converted
        ///   <see cref="FileSystemWatcherEventArgs"/> instance.
        /// </param>
        [DataTestMethod]
        [DataRow(WatcherChangeTypes.Changed, FileSystemWatcherEventType.Change)]
        [DataRow(WatcherChangeTypes.Created, FileSystemWatcherEventType.Create)]
        [DataRow(WatcherChangeTypes.Deleted, FileSystemWatcherEventType.Delete)]
        [DataRow(WatcherChangeTypes.Renamed, FileSystemWatcherEventType.Rename)]
        [DataRow(WatcherChangeTypes.All, FileSystemWatcherEventType.Undefined)]
        [DataRow(WatcherChangeTypes.Changed | WatcherChangeTypes.Created, FileSystemWatcherEventType.Undefined)]
        public void ConvertFileSystemEventArgsToFileSystemWatcherEventArgsTest(
            WatcherChangeTypes changeType,
            FileSystemWatcherEventType expectedEventType)
        {
            // Generate a new random file name and retrieve the path to the platform's temporary folder
            var randomFileName = Path.GetRandomFileName();
            var temporaryDirectory = Path.GetTempPath();

            // Create a new FileSystemEventArgs instance and convert it using the extension method under test
            var fileSystemEventArgs = new FileSystemEventArgs(changeType, temporaryDirectory, randomFileName);
            var fileSystemWatcherEventArgs = fileSystemEventArgs.ConvertToFileSystemWatcherEventArgs();

            // Verify that the fully qualified path and file or directory name are combined as FullPath of the converted instance
            Assert.AreEqual(
                temporaryDirectory + randomFileName,
                fileSystemWatcherEventArgs.FullPath,
                $"The converted {typeof(FileSystemWatcherEventArgs).Name} instance should refer to the same fully qualified path as the original {typeof(FileSystemEventArgs).Name}.");

            // Verify that the file or directory name of the converted instance matches the value of the original instance
            Assert.AreEqual(
                randomFileName,
                fileSystemWatcherEventArgs.Name,
                $"The converted {typeof(FileSystemWatcherEventArgs).Name} instance should refer to the same file or directory name as the original {typeof(FileSystemEventArgs).Name}.");

            // Verify that the value of enumeration WatcherChangeTypes has been converted to the expected value of enumeration FileSystemWatcherEventType
            Assert.AreEqual(
                expectedEventType,
                fileSystemWatcherEventArgs.EventType,
                $"The {nameof(FileSystemWatcherEventArgs.EventType)} of the converted {typeof(FileSystemWatcherEventArgs).Name} instance should match the expected value {expectedEventType}.");
        }

        /// <summary>
        ///   Verifies that all values will be preserved during the conversion of an instance of class
        ///   <see cref="RenamedEventArgs"/> to an instance of class <see cref="FileSystemWatcherEventArgs"/>
        ///   and that the <see cref="FileSystemWatcherEventType"/> of the converted instance will be set to
        ///   <see cref="FileSystemWatcherEventType.Rename"/>.
        /// </summary>
        [TestMethod]
        public void ConvertRenamedEventArgsToFileSystemWatcherRenameEventArgs()
        {
            // Generate a new random file name and retrieve the path to the platform's temporary folder
            var oldFileName = Path.GetRandomFileName();
            var newFileName = Path.GetRandomFileName();
            var temporaryDirectory = Path.GetTempPath();

            // Create a new RenamedEventArgs instance and convert it using the extension method under test
            var fileSystemEventArgs = new RenamedEventArgs(WatcherChangeTypes.Renamed, temporaryDirectory, newFileName, oldFileName);
            var fileSystemWatcherEventArgs = fileSystemEventArgs.ConvertToFileSystemWatcherEventArgs();

            // Verify that the new fully qualified path and file or directory name are combined as FullPath of the converted instance
            Assert.AreEqual(
                temporaryDirectory + newFileName,
                fileSystemWatcherEventArgs.FullPath,
                $"The converted {typeof(FileSystemWatcherEventArgs).Name} instance should refer to the same new fully qualified path as the original {typeof(RenamedEventArgs).Name}.");

            // Verify that the new file or directory name of the converted instance matches the value of the original instance
            Assert.AreEqual(
                newFileName,
                fileSystemWatcherEventArgs.Name,
                $"The converted {typeof(FileSystemWatcherEventArgs).Name} instance should refer to the same new file or directory name as the original {typeof(RenamedEventArgs).Name}.");

            // Safely cast the converted FileSystemWatcherEventArgs instance to the more specific class FileSystemWatcherRenameEventArgs
            var fileSystemWatcherRenameEventArgs = fileSystemWatcherEventArgs as FileSystemWatcherRenameEventArgs;
            Assert.IsNotNull(
                fileSystemWatcherRenameEventArgs,
                $"The converted {typeof(FileSystemWatcherEventArgs).Name} instance should be of type {typeof(FileSystemWatcherRenameEventArgs).Name}.");

            // Verify that the previous fully qualified path and file or directory name are combined as FullPath of the converted instance
            Assert.AreEqual(
                temporaryDirectory + oldFileName,
                fileSystemWatcherRenameEventArgs.PreviousFullPath,
                $"The converted {typeof(FileSystemWatcherRenameEventArgs).Name} instance should refer to the same previous fully qualified path as the original {typeof(RenamedEventArgs).Name}.");

            // Verify that the previous file or directory name of the converted instance matches the value of the original instance
            Assert.AreEqual(
                oldFileName,
                fileSystemWatcherRenameEventArgs.PreviousName,
                $"The converted {typeof(FileSystemWatcherRenameEventArgs).Name} instance should refer to the same previous file or directory name as the original {typeof(RenamedEventArgs).Name}.");
        }

        /// <summary>
        ///   Verifies that the values of the enumeration <see cref="WatcherChangeTypes"/> are converted to the expected values
        ///   of the <see cref="FileSystemWatcherEventType"/> enumeration, taking all possible types of <see cref="EventArgs"/>
        ///   provided by the <see cref="FileSystemWatcher"/> into account.
        /// </summary>
        /// <param name="eventArgsType">
        ///   Defines the type of <see cref="EventArgs"/> to be tested. A new instance of this type will be created using the
        ///   <see cref="Activator"/> class, passing the supplied array <paramref name="constructorArguments"/> to the constructor.
        /// </param>
        /// <param name="constructorArguments">
        ///   An array containing additional arguments which are passed to the constructor when dynamically creating a new
        ///   instance of type <paramref name="eventArgsType"/>.
        /// </param>
        /// <param name="expectedEventType">
        ///   The expected value of the <see cref="FileSystemWatcherEventType"/> enumeration retrieved for the dynamically
        ///   created instance of type <paramref name="eventArgsType"/>.
        /// </param>
        [DataTestMethod]
        [DataRow(typeof(FileSystemEventArgs), new object[] { WatcherChangeTypes.Created, "", "" }, FileSystemWatcherEventType.Create)]
        [DataRow(typeof(FileSystemEventArgs), new object[] { WatcherChangeTypes.Changed, "", "" }, FileSystemWatcherEventType.Change)]
        [DataRow(typeof(FileSystemEventArgs), new object[] { WatcherChangeTypes.Deleted, "", "" }, FileSystemWatcherEventType.Delete)]
        [DataRow(typeof(FileSystemEventArgs), new object[] { WatcherChangeTypes.Renamed, "", "" }, FileSystemWatcherEventType.Rename)]
        [DataRow(typeof(FileSystemEventArgs), new object[] { WatcherChangeTypes.All, "", "" }, FileSystemWatcherEventType.Undefined)]
        [DataRow(typeof(FileSystemEventArgs), new object[] { WatcherChangeTypes.Created | WatcherChangeTypes.Deleted, "", "" }, FileSystemWatcherEventType.Undefined)]
        [DataRow(typeof(FileSystemEventArgs), new object[] { WatcherChangeTypes.Created | WatcherChangeTypes.Changed | WatcherChangeTypes.Deleted, "", "" }, FileSystemWatcherEventType.Undefined)]
        [DataRow(typeof(RenamedEventArgs), new object[] { WatcherChangeTypes.Created, "", "", "" }, FileSystemWatcherEventType.Rename)]
        [DataRow(typeof(RenamedEventArgs), new object[] { WatcherChangeTypes.Changed, "", "", "" }, FileSystemWatcherEventType.Rename)]
        [DataRow(typeof(RenamedEventArgs), new object[] { WatcherChangeTypes.Deleted, "", "", "" }, FileSystemWatcherEventType.Rename)]
        [DataRow(typeof(RenamedEventArgs), new object[] { WatcherChangeTypes.Renamed, "", "", "" }, FileSystemWatcherEventType.Rename)]
        [DataRow(typeof(RenamedEventArgs), new object[] { WatcherChangeTypes.All, "", "", "" }, FileSystemWatcherEventType.Rename)]
        [DataRow(typeof(RenamedEventArgs), new object[] { WatcherChangeTypes.Created | WatcherChangeTypes.Deleted, "", "", "" }, FileSystemWatcherEventType.Rename)]
        [DataRow(typeof(RenamedEventArgs), new object[] { WatcherChangeTypes.Created | WatcherChangeTypes.Changed | WatcherChangeTypes.Deleted, "", "", "" }, FileSystemWatcherEventType.Rename)]
        [DataRow(typeof(ErrorEventArgs), new object[] { null }, FileSystemWatcherEventType.Error)]
        [DataRow(typeof(AssemblyLoadEventArgs), new object[] { null }, FileSystemWatcherEventType.Undefined)]
        [DataRow(typeof(UnhandledExceptionEventArgs), new object[] { null, true }, FileSystemWatcherEventType.Undefined)]
        public void GetFileSystemWatcherEventTypeFromEventArgs(Type eventArgsType, object[] constructorArguments, FileSystemWatcherEventType expectedEventType)
        {
            // Verify that the supplied type is derived from 'System.EventArgs'
            Assert.IsTrue(
                typeof(EventArgs).IsAssignableFrom(eventArgsType),
                $"The supplied type {eventArgsType.FullName} should be derived from {typeof(EventArgs).FullName}.");

            // Create a new instance of the supplied type passing the supplied arguments to its constructor
            var instance = Activator.CreateInstance(eventArgsType, constructorArguments);
            Assert.IsNotNull(instance, $"The newly created instance of type {eventArgsType.FullName} should not be null.");

            // Retrieve the FileSystemWatcherEventType from the supplied EventArgs instance
            var eventType = FileSystemEventArgsExtensions.GetFileSystemWatcherEventType((EventArgs)instance);
            Assert.AreEqual(
                expectedEventType,
                eventType,
                $"The {typeof(FileSystemWatcherEventType).Name} value {eventType} retrieved from an instance of type {eventArgsType.FullName} should match the expected value {expectedEventType}.");
        }
    }
}
