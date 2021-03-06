﻿// <copyright file="FileSystemWatcherImplementation.cs">
//   Copyright (c) 2018 LightSlateGray
// </copyright>
// <author>
//   LightSlateGray
// </author>
// <date>
//   2018-04-03
// </date>
// <summary>
//   An implementation of the IFileSystemWatcher interface, which is used internally by the FileSystemWatcherFactory.
// </summary>
// <license>
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//   
//     http://www.apache.org/licenses/LICENSE-2.0
//   
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </license>

namespace LightSlateGray.FileSystemWatcher.Implementations
{
    using LightSlateGray.FileSystemWatcher.Enumerations;
    using LightSlateGray.FileSystemWatcher.Interfaces;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Threading;

    /// <summary>
    ///   An implementation of the <see cref="IFileSystemWatcher"/> interface, which is used internally by the <see cref="FileSystemWatcherFactory"/>.
    /// </summary>
    sealed class FileSystemWatcherImplementation : IFileSystemWatcher
    {
        /// <summary>
        ///   A simple <see cref="Object"/> instance used to synchronize access to the <see cref="fileSystemWatcher"/> instance
        ///   while querying or updating the active state and the event handler of the internal <see cref="FileSystemWatcher"/>.
        /// </summary>
        private readonly object synchronizationLock = new object();

        /// <summary>
        ///   A class-private instance of the <see cref="FileSystemWatcher"/> instance, created within the constructor,
        ///   which will actively watch for changes and invoke the implementation of the <see cref="IFileSystemWatcherEventHandler"/>
        ///   interface stored within property <see cref="FileSystemWatcherEventHandler"/>.
        /// </summary>
        private readonly FileSystemWatcher fileSystemWatcher;

        /// <summary>
        ///   The backing field of property <see cref="FileSystemWatcherEventHandler"/>.
        /// </summary>
        private IFileSystemWatcherEventHandler fileSystemWatcherEventHandler;

        /// <summary>
        ///   Creates an instance of the <see cref="FileSystemWatcherImplementation"/> class for the supplied
        ///   <paramref name="path"/> if the argument references an existing <see cref="File"/> or <see cref="Directory"/>.
        /// </summary>
        /// <param name="path">
        ///   The path of the <see cref="File"/> or <see cref="Directory"/> to watch for changes.
        ///   Might throw an <see cref="ArgumentException"/> if the <paramref name="path"/> neither refers to an existing file,
        ///   nor an existing directory.
        /// </param>
        /// <param name="fileWatcherEventHandler">
        ///   An implementation of the <see cref="IFileWatcherEventHandler"/> interface that will be set up
        ///   to get notified for events of the newly created <see cref="FileSystemWatcherImplementation"/> instance.
        /// </param>
        /// <returns>
        ///   A new instance of the <see cref="FileSystemWatcherImplementation"/> class if the supplied <paramref name="path"/>
        ///   is valid, otherwise <code>null</code>.
        /// </returns>
        public FileSystemWatcherImplementation(string path, IFileSystemWatcherEventHandler fileWatcherEventHandler = null)
        {
            // Store a reference to the supplied implementation of the IFileSystemWatcherEventHandler interface internally
            this.FileSystemWatcherEventHandler = fileWatcherEventHandler;

            // Trim the supplied path or fall back to an empty string
            path = path?.Trim() ?? string.Empty;

            // Check if the supplied path refers to an existing file or an existing directory
            var isExistingFile = File.Exists(path);
            var isExistingDirectory = Directory.Exists(path);

            // Throw an exception if the supplied path neither references a file nor a folder
            if (!isExistingFile && !isExistingDirectory)
            {
                throw new ArgumentException($"The supplied path neither is a directory, nor a file: '{path}'.", nameof(path));
            }

            // Set the FileSystemWatcherType accordingly
            this.FileSystemWatcherType = isExistingFile ? FileSystemWatcherType.File : FileSystemWatcherType.Directory;

            // Create a new FileSystemWatcher instance using the object initializer syntax, which is atomic by design
            this.fileSystemWatcher = new FileSystemWatcher()
            {
                Path = isExistingFile ? Directory.GetParent(path).FullName : path,
                Filter = isExistingFile ? Path.GetFileName(path) : null,
                NotifyFilter = NotifyFilters.LastWrite | (isExistingFile ? NotifyFilters.FileName : NotifyFilters.DirectoryName)
            };
        }

        /// <summary>
        ///   Gets the name of the file or folder being watched by the implementation of the
        ///   <see cref="IFileSystemWatcher"/> interface. This includes the file extension, if any.
        /// </summary>
        public string Name => this.FileSystemWatcherType == FileSystemWatcherType.File
            ? this.fileSystemWatcher.Filter
            : Path.GetFileName(this.fileSystemWatcher.Path);

        /// <summary>
        ///   Gets the full path to the file or folder being watched by the implementation of the
        ///   <see cref="IFileSystemWatcher"/> interface. This includes the file name and its extension, if any.
        /// </summary>
        public string FullPath => this.FileSystemWatcherType == FileSystemWatcherType.File
            ? this.fileSystemWatcher.Path + Path.DirectorySeparatorChar + this.Name
            : this.fileSystemWatcher.Path;

        /// <summary>
        ///   Gets the Multipurpose Internet Mail Extensions (MIME) type of the file being watched. Will return the generic
        ///   value <c>application/octet-stream</c> if a directory is being watched (i.e. <see cref="FileSystemWatcherType"/>
        ///   is set to <see cref="FileSystemWatcherType.Directory"/>).
        /// </summary>
        public string MimeType => this.FileSystemWatcherType == FileSystemWatcherType.Directory
            ? "application/octet-stream"
            : Path.GetExtension(this.Name); // TODO (Sam): Implement MIME type retrieval.

        /// <summary>
        ///   Gets the enumeration value identifying the type currently being watched.
        /// </summary>
        public FileSystemWatcherType FileSystemWatcherType { get; private set; } = FileSystemWatcherType.Unknown;

        /// <summary>
        ///   Gets or sets an implementation of the <see cref="IFileSystemWatcherEventHandler"/> interface of which its
        ///   corresponding event handler callback methods will be invoked if a change has been detected.
        /// </summary>
        public IFileSystemWatcherEventHandler FileSystemWatcherEventHandler
        {
            get => this.fileSystemWatcherEventHandler;
            set
            {
                // Retrieve a value, indicating whether the FileSystemWatcher instance is currently active
                var isWatching = this.IsWatching;

                // Remove the registrations for the implementation of the IFileSystemWatcherEventHandler interface that will be removed
                if (isWatching)
                {
                    this.StopWatching();
                }

                // TODO (Sam): Remove the possibility of event loss by not stop listening and collecting occurred events until the new handler is set

                // Update the implementation of the IFileSystemWatcherEventHandler interface
                this.fileSystemWatcherEventHandler = value;

                // Register the new implementation of the IFileSystemWatcherEventHandler interface if the previous handler was already listening
                if (isWatching)
                {
                    this.StartWatching();
                }
            }
        }

        /// <summary>
        ///   Gets a value indicating whether the implementation of the <see cref="IFileSystemWatcher"/> interface is
        ///   actively watching for changes, or currently disabled.
        /// </summary>
        public bool IsWatching
        {
            get
            {
                // Gain exclusive access to the internal FileSystemWatcher instance, because its property might be updated
                lock (this.synchronizationLock)
                {
                    return this.fileSystemWatcher?.EnableRaisingEvents ?? false;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the <see cref="ISite"/> associated with the implementation of the <see cref="IFileSystemWatcher"/> interface.
        /// </summary>
        public ISite Site { get; set; }

        /// <summary>
        ///   Starts up the implementation of the <see cref="IFileSystemWatcher"/> interface, which will set the value of
        ///   property <see cref="IsWatching"/> to <c>true</c> and trigger the corresponding callback methods of the
        ///   <see cref="IFileSystemWatcherEventHandler"/> for each detected change.
        /// </summary>
        public void StartWatching()
        {
            // Synchronize access to the event handler registrations, so that this method becomes thread-safe
            lock (this.synchronizationLock)
            {
                if (this.fileSystemWatcher != null)
                {
                    /*
                     * TODO (Sam): Ensure that only a single notification will be triggered for each change
                     */

                    // Wire up the events with the internal FileSystemWatcher instance
                    this.fileSystemWatcher.Changed += this.OnChanged;
                    this.fileSystemWatcher.Created += this.OnCreated;
                    this.fileSystemWatcher.Deleted += this.OnDeleted;
                    this.fileSystemWatcher.Error += this.OnError;
                    this.fileSystemWatcher.Renamed += this.OnRenamed;

                    // Let the internal FileSystemWatcher instance raise its events
                    this.fileSystemWatcher.EnableRaisingEvents = true;
                }
            }
        }

        /// <summary>
        ///   Stops the implementation of the <see cref="IFileSystemWatcher"/> interface, which will set the value of
        ///   property <see cref="IsWatching"/> to <c>false</c> and prevent the implementation of the 
        ///   <see cref="IFileSystemWatcherEventHandler"/> interface from triggering any callback methods.
        /// </summary>
        public void StopWatching()
        {
            // Synchronize access to the removal of the event handler registrations, so that this method becomes thread-safe
            lock (this.synchronizationLock)
            {
                if (this.fileSystemWatcher != null)
                {
                    // Prevent the internal FileSystemWatcher instance from raising any further events
                    this.fileSystemWatcher.EnableRaisingEvents = false;

                    // Unwire the events from the internal FileSystemWatcher instance
                    fileSystemWatcher.Changed -= this.OnChanged;
                    fileSystemWatcher.Created -= this.OnCreated;
                    fileSystemWatcher.Deleted -= this.OnDeleted;
                    fileSystemWatcher.Error -= this.OnError;
                    fileSystemWatcher.Renamed -= this.OnRenamed;
                }
           }
        }

        #region IFileSystemWatcherEventHandler methods

        /// <summary>
        ///   Event handler method for the <see cref="FileSystemWatcher.Changed"/> event.
        /// </summary>
        /// <param name="sender">
        ///   The sender of the event. Should be an instance of the <see cref="FileSystemWatcher"/> class.
        /// </param>
        /// <param name="eventArgs">
        ///   An instance of class <see cref="FileSystemEventArgs"/> containing information about the change.
        /// </param>
        public void OnChanged(object sender, FileSystemEventArgs eventArgs)
        {
            this.FileSystemWatcherEventHandler?.OnChanged(this, eventArgs);
        }

        /// <summary>
        ///   Event handler method for the <see cref="FileSystemWatcher.Created"/> event.
        /// </summary>
        /// <param name="sender">
        ///   The sender of the event. Should be an instance of the <see cref="FileSystemWatcher"/> class.
        /// </param>
        /// <param name="eventArgs">
        ///   An instance of class <see cref="FileSystemEventArgs"/> containing information about the
        ///   newly created file or directory.
        /// </param>
        public void OnCreated(object sender, FileSystemEventArgs eventArgs)
        {
            this.FileSystemWatcherEventHandler?.OnCreated(this, eventArgs);
        }

        /// <summary>
        ///   Event handler method for the <see cref="FileSystemWatcher.Deleted"/> event.
        /// </summary>
        /// <param name="sender">
        ///   The sender of the event. Should be an instance of the <see cref="FileSystemWatcher"/> class.
        /// </param>
        /// <param name="eventArgs">
        ///   An instance of class <see cref="FileSystemEventArgs"/> containing information about the
        ///   file or directory that has been deleted.
        /// </param>
        public void OnDeleted(object sender, FileSystemEventArgs eventArgs)
        {
            this.FileSystemWatcherEventHandler?.OnDeleted(this, eventArgs);
        }

        /// <summary>
        ///   Event handler method for the <see cref="FileSystemWatcher.Error"/> event.
        /// </summary>
        /// <param name="sender">
        ///   The sender of the event. Should be an instance of the <see cref="FileSystemWatcher"/> class.
        /// </param>
        /// <param name="eventArgs">
        ///   An instance of class <see cref="System.IO.ErrorEventArgs"/> containing the <see cref="System.Exception"/>
        ///   that has occurred while trying to watch for changes to a specific file or directory.
        /// </param>
        public void OnError(object sender, ErrorEventArgs eventArgs)
        {
            this.FileSystemWatcherEventHandler?.OnError(this, eventArgs);
        }

        /// <summary>
        ///   Event handler method for the <see cref="FileSystemWatcher.Renamed"/> event.
        /// </summary>
        /// <param name="sender">
        ///   The sender of the event. Should be an instance of the <see cref="FileSystemWatcher"/> class.
        /// </param>
        /// <param name="eventArgs">
        ///   An instance of class <see cref="RenamedEventArgs"/> containing the previous and new name
        ///   of the file or directory that has been renamed.
        /// </param>
        public void OnRenamed(object sender, RenamedEventArgs eventArgs)
        {
            this.FileSystemWatcherEventHandler?.OnRenamed(this, eventArgs);
        }

        #endregion

        #region IDisposable Support

        /// <summary>
        ///   An integer value, indicating whether this <see cref="FileSystemWatcherImplementation"/> instance has already been disposed.
        /// </summary>
        private int disposeCalls = 0;

        /// <summary>
        ///   An <see cref="EventHandler"/> to notify observers about the disposal of the <see cref="FileSystemWatcherImplementation"/> instance.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        ///   Provides a mechanism for deterministic finalization of the <see cref="FileSystemWatcherImplementation"/> instance by releasing
        ///   any resources associated with the <see cref="FileSystemWatcherImplementation"/> instance.
        /// </summary>
        public void Dispose()
        {
            // Thread-safe comparison of the integer for equality with 0; if equal, replaces the it with value 1, while returning the original value
            if (0 == Interlocked.CompareExchange(ref this.disposeCalls, 1, 0))
            {
                // Stop watching for changes of the internal FileSystemWatcher instance
                this.StopWatching();

                // Clear the reference to the implementation of the IFileSystemWatcherEventHandler interface
                this.FileSystemWatcherEventHandler = null;

                // Notify all observers about the disposal of this instance
                this.Disposed?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}
