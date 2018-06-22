/// <copyright file="IFileSystemWatcherEventHandler.cs">
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
    using LightSlateGray.FileSystemWatcher.Events;

    /// <summary>
    ///   An interface defining methods to handle events invoked by instances of the
    ///   <see cref="System.IO.FileSystemWatcher"/> class.
    /// </summary>
    public interface IFileSystemWatcherEventHandler
    {
        /// <summary>
        ///   Event handler method for any kind of change to the file or directory being watched.
        ///   Further information about the type of change is provided via <see cref="FileSystemWatcherEventArgs.EventType"/>.
        /// </summary>
        /// <param name="sender">
        ///   The sender of the event. Should be an instance of the <see cref="System.IO.FileSystemWatcher"/> class.
        /// </param>
        /// <param name="eventArgs">
        ///   An instance of class <see cref="FileSystemWatcherEventArgs"/> containing information about the change and the file
        ///   or directory being affected.
        /// </param>
        void OnFileSystemWatcherEvent(object sender, FileSystemWatcherEventArgs eventArgs);
    }
}