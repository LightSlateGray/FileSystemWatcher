/// <copyright file="IFileSystemWatcher.cs">
///   Copyright (c) 2018 LightSlateGray
/// </copyright>
/// <author>
///   LightSlateGray
/// </author>
/// <date>
///   2018-04-03
/// </date>
/// <summary>
///   An interface providing some of the functionality of the <see cref="System.IO.FileSystemWatcher"/> class.
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

namespace LightSlateGray.FileSystemWatcher.Interfaces
{
    using LightSlateGray.FileSystemWatcher.Enumerations;
    using System;
    using System.ComponentModel;

    /// <summary>
    ///   An interface providing some of the functionality of the <see cref="System.IO.FileSystemWatcher"/> class.
    /// </summary>
    public interface IFileSystemWatcher : IFileSystemWatcherEventHandler, IComponent
    {
        /// <summary>
        ///   Gets the name of the file or folder being watched by the implementation of the
        ///   <see cref="IFileSystemWatcher"/> interface. This includes the file extension, if any.
        /// </summary>
        String Name { get; }

        /// <summary>
        ///   Gets the full path to the file or folder being watched by the implementation of the
        ///   <see cref="IFileSystemWatcher"/> interface. This includes the file name and its extension, if any.
        /// </summary>
        String FullPath { get; }

        /// <summary>
        ///   Gets the Multipurpose Internet Mail Extensions (MIME) type of the file being watched. Will return the generic
        ///   value <c>application/octet-stream</c> if a directory is being watched (i.e. <see cref="FileSystemWatcherType"/>
        ///   is set to <see cref="FileSystemWatcherType.Directory"/>).
        /// </summary>
        String MimeType { get; }

        /// <summary>
        ///   Gets or sets an implementation of the <see cref="IFileSystemWatcherEventHandler"/> interface of which its
        ///   corresponding event handler callback methods will be invoked if a change has been detected.
        /// </summary>
        IFileSystemWatcherEventHandler FileSystemWatcherEventHandler { get; set; }

        /// <summary>
        ///   Gets the enumeration value identifying the type currently being watched.
        /// </summary>
        FileSystemWatcherType FileSystemWatcherType { get; }

        /// <summary>
        ///   Gets a value indicating whether the implementation of the <see cref="IFileSystemWatcher"/> interface is
        ///   actively watching for changes, or currently disabled.
        /// </summary>
        bool IsWatching { get; }

        /// <summary>
        ///   Starts up the implementation of the <see cref="IFileSystemWatcher"/> interface, which will set the value of
        ///   property <see cref="IsWatching"/> to <c>true</c> and trigger the corresponding callback methods of the
        ///   <see cref="IFileSystemWatcherEventHandler"/> for each detected change.
        /// </summary>
        void StartWatching();

        /// <summary>
        ///   Stops the implementation of the <see cref="IFileSystemWatcher"/> interface, which will set the value of
        ///   property <see cref="IsWatching"/> to <c>false</c> and prevent the implementation of the 
        ///   <see cref="IFileSystemWatcherEventHandler"/> interface from triggering any callback methods.
        /// </summary>
        void StopWatching();
    }
}
