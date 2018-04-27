/// <copyright file="IFileWatcherEventHandler.cs">
///   Copyright (c) 2018 LightSlateGray
/// </copyright>
/// <author>
///   LightSlateGray
/// </author>
/// <date>
///   2018-01-29
/// </date>
/// <summary>
///   An interface defining methods to handle events invoked by instances of the <see cref="System.IO.FileSystemWatcher"/> class.
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
    /// <summary>
    ///   An interface defining methods to handle events invoked by instances of the
    ///   <see cref="System.IO.FileSystemWatcher"/> class.
    /// </summary>
    public interface IFileSystemWatcherEventHandler
    {
        /// <summary>
        ///   Event handler method for the <see cref="System.IO.FileSystemWatcher.Changed"/> event.
        /// </summary>
        /// <param name="sender">
        ///   The sender of the event. Should be an instance of the <see cref="System.IO.FileSystemWatcher"/> class.
        /// </param>
        /// <param name="eventArgs">
        ///   An instance of class <see cref="System.IO.FileSystemEventArgs"/> containing information about the change.
        /// </param>
        void OnChanged(object sender, System.IO.FileSystemEventArgs eventArgs);

        /// <summary>
        ///   Event handler method for the <see cref="System.IO.FileSystemWatcher.Created"/> event.
        /// </summary>
        /// <param name="sender">
        ///   The sender of the event. Should be an instance of the <see cref="System.IO.FileSystemWatcher"/> class.
        /// </param>
        /// <param name="eventArgs">
        ///   An instance of class <see cref="System.IO.FileSystemEventArgs"/> containing information about the
        ///   newly created file or directory.
        /// </param>
        void OnCreated(object sender, System.IO.FileSystemEventArgs eventArgs);

        /// <summary>
        ///   Event handler method for the <see cref="System.IO.FileSystemWatcher.Deleted"/> event.
        /// </summary>
        /// <param name="sender">
        ///   The sender of the event. Should be an instance of the <see cref="System.IO.FileSystemWatcher"/> class.
        /// </param>
        /// <param name="eventArgs">
        ///   An instance of class <see cref="System.IO.FileSystemEventArgs"/> containing information about the
        ///   file or directory that has been deleted.
        /// </param>
        void OnDeleted(object sender, System.IO.FileSystemEventArgs eventArgs);

        /// <summary>
        ///   Event handler method for the <see cref="System.IO.FileSystemWatcher.Error"/> event.
        /// </summary>
        /// <param name="sender">
        ///   The sender of the event. Should be an instance of the <see cref="System.IO.FileSystemWatcher"/> class.
        /// </param>
        /// <param name="eventArgs">
        ///   An instance of class <see cref="System.IO.ErrorEventArgs"/> containing the <see cref="System.Exception"/>
        ///   that has occurred while trying to watch for changes to a specific file or directory.
        /// </param>
        void OnError(object sender, System.IO.ErrorEventArgs eventArgs);

        /// <summary>
        ///   Event handler method for the <see cref="System.IO.FileSystemWatcher.Renamed"/> event.
        /// </summary>
        /// <param name="sender">
        ///   The sender of the event. Should be an instance of the <see cref="System.IO.FileSystemWatcher"/> class.
        /// </param>
        /// <param name="eventArgs">
        ///   An instance of class <see cref="System.IO.RenamedEventArgs"/> containing the previous and new name
        ///   of the file or directory that has been renamed.
        /// </param>
        void OnRenamed(object sender, System.IO.RenamedEventArgs eventArgs);
    }
}