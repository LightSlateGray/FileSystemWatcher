/// <copyright file="FileSystemEventArgsExtensions.cs">
///   Copyright (c) 2018 LightSlateGray
/// </copyright>
/// <author>
///   LightSlateGray
/// </author>
/// <date>
///   2018-06-03
/// </date>
/// <summary>
///   Extension methods for class FileSystemEventArgs.
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

namespace LightSlateGray.FileSystemWatcher.Extensions
{
    using Events;
    using LightSlateGray.FileSystemWatcher.Enumerations;
    using System.IO;

    /// <summary>
    ///   Extension methods for class <see cref="FileSystemEventArgs"/>.
    /// </summary>
    public static class FileSystemEventArgsExtensions
    {
        /// <summary>
        ///   Converts an instance of class <see cref="FileSystemEventArgs"/> to an instance of class
        ///   <see cref="FileSystemEventArgs"/>, populating the new instance using properties of the supplied instance.
        /// </summary>
        /// <param name="fileSystemEventArgs">
        ///   The instance of class <see cref="FileSystemEventArgs"/> convert to
        ///   an instance of class <see cref="FileSystemWatcherEventArgs"/>.
        /// </param>
        /// <returns>
        ///   A new instance of class <see cref="FileSystemWatcherEventArgs"/> populated with values provided by a
        ///   <see cref="FileSystemEventArgs"/> instance.
        /// </returns>
        public static FileSystemWatcherEventArgs ConvertToFileSystemWatcherEventArgs(this FileSystemEventArgs fileSystemEventArgs)
        {
            // Check if the supplied FileSystemEventArgs instance has been issued due to renaming and convert appropriately
            if (fileSystemEventArgs is RenamedEventArgs renamedEventArgs)
            {
                return new FileSystemWatcherRenameEventArgs(
                    renamedEventArgs.FullPath,
                    renamedEventArgs.Name,
                    renamedEventArgs.OldFullPath,
                    renamedEventArgs.OldName);
            }

            // Create a new generic FileSystemWatcherEventArgs instance populated with data from the supplied FileSysteEventArgs instance
            return new FileSystemWatcherEventArgs(
                GetFileSystemWatcherEventType(fileSystemEventArgs),
                fileSystemEventArgs.FullPath,
                fileSystemEventArgs.Name);
        }

        /// <summary>
        ///   Retrieves the corresponding <see cref="FileSystemWatcherEventType"/> from the supplied <paramref name="eventArgs"/>.
        ///   If the supplied <paramref name="eventArgs"/> is neither an instance of class <see cref="FileSystemEventArgs"/> nor
        ///   of class <see cref="ErrorEventArgs"/>, <see cref="FileSystemWatcherEventType.Undefined"/> is returned.
        /// </summary>
        /// <param name="eventArgs">
        ///   An instance of type <see cref="System.EventArgs"/> for which the corresponding
        ///   <see cref="FileSystemWatcherEventType"/> is requested.
        /// </param>
        /// <returns>
        ///   The corresponding <see cref="FileSystemWatcherEventType"/> from the supplied <paramref name="eventArgs"/>.
        ///   If the supplied <paramref name="eventArgs"/> is neither an instance of class <see cref="FileSystemEventArgs"/> nor
        ///   of class <see cref="ErrorEventArgs"/>, <see cref="FileSystemWatcherEventType.Undefined"/> is returned.
        /// </returns>
        internal static FileSystemWatcherEventType GetFileSystemWatcherEventType(System.EventArgs eventArgs)
        {
            // Return value 'Error' if the supplied EventArgs instance is of type ErrorEventArgs
            if (eventArgs is ErrorEventArgs)
            {
                return FileSystemWatcherEventType.Error;
            }

            // Return value 'Rename' if the supplied EventArgs instance is of type RenamedEventArgs
            if (eventArgs is RenamedEventArgs)
            {
                return FileSystemWatcherEventType.Rename;
            }

            // Check if the supplied EventArgs instance is of type FileSystemEventArgs
            if (eventArgs is FileSystemEventArgs fileSystemEventArgs)
            {
                // Helper variable
                var changeType = fileSystemEventArgs.ChangeType;

                // Check enumeration value of property ChangeType which internally uses flags to indicate the type of change
                if ((changeType ^ WatcherChangeTypes.Changed) == 0)
                {
                    return FileSystemWatcherEventType.Change;
                }
                else if ((changeType ^ WatcherChangeTypes.Created) == 0)
                {
                    return FileSystemWatcherEventType.Create;
                }
                else if((changeType ^ WatcherChangeTypes.Deleted) == 0)
                {
                    return FileSystemWatcherEventType.Delete;
                }
                else if ((changeType ^ WatcherChangeTypes.Renamed) == 0)
                {
                    return FileSystemWatcherEventType.Rename;
                }
            }

            // Return value 'Undefined' if the supplied EventArgs instance is of any other type
            return FileSystemWatcherEventType.Undefined;
        }
    }
}
