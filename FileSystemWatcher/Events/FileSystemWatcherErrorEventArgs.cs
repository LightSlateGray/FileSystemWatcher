/// <copyright file="FileSystemWatcherErrorEventArgs.cs">
///   Copyright (c) 2018 LightSlateGray
/// </copyright>
/// <author>
///   LightSlateGray
/// </author>
/// <date>
///   2018-06-03
/// </date>
/// <summary>
///   Event arguments supplied to the event handler callback method defined by the IFileSystemWatcherEventHandler interface
///   indicating that an unrecoverable error occurred while watching the file or directory.
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

namespace LightSlateGray.FileSystemWatcher.Events
{
    using System;
    using LightSlateGray.FileSystemWatcher.Enumerations;

    /// <summary>
    ///   Event arguments for event type <see cref="FileSystemWatcherEventType.Error"/>, supplied to the event handler callback method
    ///   <see cref="Interfaces.IFileSystemWatcherEventHandler.OnFileSystemWatcherEvent(object, FileSystemWatcherEventArgs)"/>
    ///   defined by the <see cref="Interfaces.IFileSystemWatcherEventHandler"/> interface in order to further specify what kind of
    ///   error occurred during watching for changes.
    /// </summary>
    public class FileSystemWatcherErrorEventArgs : FileSystemWatcherEventArgs
    {
        /// <summary>
        ///   Gets the <see cref="Exception"/> that occurred during watching for changes to the file or directory provided by
        ///   <see cref="FileSystemWatcherEventArgs.FullPath"/>.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        ///   Creates a new instance of class <see cref="FileSystemWatcherErrorEventArgs"/> from the supplied arguments.
        /// </summary>
        /// <param name="fullPath">
        ///   The fully qualified name of the file or directory for which the supplied <paramref name="exception"/> occurred.
        /// </param>
        /// <param name="name">
        ///   The name of the file or directory for which the supplied <paramref name="exception"/> occurred.
        /// </param>
        /// <param name="exception">
        ///   Contains the <see cref="Exception"/> that occurred while watching for changes of the file or directory defined by
        ///   <paramref name="fullPath"/> and <paramref name="name"/>.
        /// </param>
        public FileSystemWatcherErrorEventArgs(string fullPath, string name, Exception exception)
            : base(FileSystemWatcherEventType.Error, fullPath, name)
        {
            this.Exception = exception;
        }
    }
}
