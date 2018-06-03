/// <copyright file="FileSystemWatcherEventArgs.cs">
///   Copyright (c) 2018 LightSlateGray
/// </copyright>
/// <author>
///   LightSlateGray
/// </author>
/// <date>
///   2018-06-03
/// </date>
/// <summary>
///   Event arguments supplied to the event handler callback method defined by the IFileSystemWatcherEventHandler interface.
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
    ///   Event arguments supplied to the event handler callback method
    ///   <see cref="Interfaces.IFileSystemWatcherEventHandler.OnFileSystemWatcherEvent(object, FileSystemWatcherEventArgs)"/>
    ///   defined by the <see cref="Interfaces.IFileSystemWatcherEventHandler"/> interface.
    /// </summary>
    public class FileSystemWatcherEventArgs : EventArgs
    {
        /// <summary>
        ///   Gets the type of event that occurred for the file or directory being watched.
        /// </summary>
        public FileSystemWatcherEventType EventType { get; }

        /// <summary>
        ///   Gets the fully qualified path of the file or directory affected by the event.
        /// </summary>
        public string FullPath { get; }

        /// <summary>
        ///   Gets the name of the file or directory affected by the event.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///   Creates a new instance of class <see cref="FileSystemWatcherEventArgs"/> from the supplied arguments.
        /// </summary>
        /// <param name="eventType">
        ///   Defines the type of the event.
        /// </param>
        /// <param name="fullPath">
        ///   The fully qualified name of the file or directory for which the supplied <paramref name="eventType"/> occurred.
        /// </param>
        /// <param name="name">
        ///   The name of the file or directory for which the supplied <paramref name="eventType"/> occurred.
        /// </param>
        public FileSystemWatcherEventArgs(FileSystemWatcherEventType eventType, string fullPath, string name)
        {
            this.EventType = eventType;
            this.FullPath = fullPath;
            this.Name = name;
        }
    }
}
