/// <copyright file="FileSystemWatcherEventType.cs">
///   Copyright (c) 2018 LightSlateGray
/// </copyright>
/// <author>
///   LightSlateGray
/// </author>
/// <date>
///   2018-06-03
/// </date>
/// <summary>
///   An enumeration defining the available types of events supported by the IFileSystemWatcher interface.
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

namespace LightSlateGray.FileSystemWatcher.Enumerations
{
    /// <summary>
    ///   An enumeration defining the available types of events supported by the <see cref="IFileSystemWatcher"/> interface.
    /// </summary>
    public enum FileSystemWatcherEventType
    {
        /// <summary>
        ///   Defines a default value for change types which are not (yet) supported.
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///   Indicates that the contents of the file or directory being watched have changed.
        /// </summary>
        Change = 1,

        /// <summary>
        ///   Indicates that the non-existent file being watched has been created or
        ///   that a new file or directory within the directory being watched has been created.
        /// </summary>
        Create = 2,

        /// <summary>
        ///   Indicates that the file being watched has been deleted from the file system or
        ///   that a file or directory within the directory being watched has been deleted.
        /// </summary>
        Delete = 3,

        /// <summary>
        ///   Indicates that the file or directory being watched has been renamed or
        ///   moved to another location within the file system.
        /// </summary>
        Rename = 4,

        /// <summary>
        ///   Indicates that an unrecoverable error has been encountered while watching the file or directory.
        /// </summary>
        Error = 5
    }
}
