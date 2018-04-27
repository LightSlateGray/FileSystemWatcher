/// <copyright file="FileSystemWatcherType.cs">
///   Copyright (c) 2018 LightSlateGray
/// </copyright>
/// <author>
///   LightSlateGray
/// </author>
/// <date>
///   2018-04-03
/// </date>
/// <summary>
///   An enumeration defining the watchable types.
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
    ///   An enumeration containing definitions for watchable types.
    /// </summary>
    public enum FileSystemWatcherType
    {
        /// <summary>
        ///   Defines a default values for values that are not (yet) supported.
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///   Indicates, that the watchable type is a file.
        /// </summary>
        File = 1,

        /// <summary>
        ///   Indicates, that the watchable type is a directory.
        /// </summary>
        Directory = 2
    }
}
