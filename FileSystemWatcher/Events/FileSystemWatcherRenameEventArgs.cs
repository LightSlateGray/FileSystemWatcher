/// <copyright file="FileSystemWatcherRenameEventArgs.cs">
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
///   when the name of a file or directory being watched has been changed.
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
    using LightSlateGray.FileSystemWatcher.Enumerations;

    /// <summary>
    ///   Event arguments for event type <see cref="FileSystemWatcherEventType.Rename"/>, supplied to the event handler callback method
    ///   <see cref="Interfaces.IFileSystemWatcherEventHandler.OnFileSystemWatcherEvent(object, FileSystemWatcherEventArgs)"/>
    ///   defined by the <see cref="Interfaces.IFileSystemWatcherEventHandler"/> interface if the file or directory being watched
    ///   has been renamed.
    /// </summary>
    public class FileSystemWatcherRenameEventArgs : FileSystemWatcherEventArgs
    {
        /// <summary>
        ///   Gets the fully qualified path of the file or directory used before it had been affected by the event.
        /// </summary>
        public string PreviousFullPath { get; }

        /// <summary>
        ///   Gets the name of the file or directory used before it had been affected by the event.
        /// </summary>
        public string PreviousName { get; }

        /// <summary>
        ///   Creates a new instance of class <see cref="FileSystemWatcherRenameEventArgs"/> from the supplied arguments.
        /// </summary>
        /// <param name="currentFullPath">
        ///   The current or new fully qualified name of the file or directory being watched
        ///   which has been renamed or moved from <paramref name="previousFullPath"/>.
        /// </param>
        /// <param name="currentName">
        ///   The current or new name of the file or directory being watched which has been renamed from <see cref="previousName"/>.
        /// </param>
        /// <param name="previousFullPath">
        ///   The previously used fully qualified name of the file or directory being watched
        ///   which has been moved or renamed to <paramref name="currentFullPath"/>.
        /// </param>
        /// <param name="previousName">
        ///   The previously used name of the file or directory being watched which has been renamed to <paramref name="currentName"/>.
        /// </param>
        public FileSystemWatcherRenameEventArgs(
            string currentFullPath,
            string currentName,
            string previousFullPath,
            string previousName)
            : base(FileSystemWatcherEventType.Rename, currentFullPath, currentName)
        {
            PreviousFullPath = previousFullPath;
            PreviousName = previousName;
        }
    }
}
