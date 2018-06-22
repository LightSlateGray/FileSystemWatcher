/// <copyright file="FileSystemWatcherFactory.cs">
///   Copyright (c) 2018-2018 LightSlateGray
/// </copyright>
/// <author>
///   LightSlateGray
/// </author>
/// <date>
///   2018-04-27
/// </date>
/// <summary>
///   A factory that creates implementations of the <see cref="LightSlateGray.FileSystemWatcher.Interfaces.IFileSystemWatcher"/>
///   interface in a synchronous or asynchronous manner.
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

namespace LightSlateGray.FileSystemWatcher
{
    using LightSlateGray.FileSystemWatcher.Interfaces;
    using LightSlateGray.FileSystemWatcher.Implementations;
    using System.Threading.Tasks;

    /// <summary>
    ///   A factory that produces implementations of the <see cref="IFileSystemWatcher"/> interface.
    /// </summary>
    public static class FileSystemWatcherFactory
    {
        /// <summary>
        ///   Synchronously creates a new implementation of the <see cref="IFileSystemWatcher"/> interface watching the supplied
        ///   <paramref name="path"/> and invoking the callback methods of the supplied <paramref name="fileWatcherEventHandler"/>
        ///   if a change has been detected.
        /// </summary>
        /// <param name="path">
        ///   The path of the file or directory to watch.
        /// </param>
        /// <param name="fileWatcherEventHandler">
        ///   An implementation of the <see cref="IFileSystemWatcherEventHandler"/> interface to get notified when changes occur.
        ///   Might be <c>null</c> and can be set later on using property <see cref="IFileSystemWatcher.FileSystemWatcherEventHandler"/>.
        /// </param>
        /// <returns>
        ///   A new implementation of the <see cref="IFileSystemWatcher"/> interface to watch the supplied <paramref name="path"/>.
        /// </returns>
        public static IFileSystemWatcher CreateInstance(string path, IFileSystemWatcherEventHandler fileWatcherEventHandler = null)
            => new FileSystemWatcherImplementation(path, fileWatcherEventHandler);

        /// <summary>
        ///   Asynchronously creates a new implementation of the <see cref="IFileSystemWatcher"/> interface watching the supplied
        ///   <paramref name="path"/> and invoking the callback methods of the supplied <paramref name="fileWatcherEventHandler"/>
        ///   if a change has been detected.
        /// </summary>
        /// <param name="path">
        ///   The path of the file or directory to watch.
        /// </param>
        /// <param name="fileWatcherEventHandler">
        ///   An implementation of the <see cref="IFileSystemWatcherEventHandler"/> interface to get notified when changes occur.
        ///   Might be <c>null</c> and can be set later on using property <see cref="IFileSystemWatcher.FileSystemWatcherEventHandler"/>.
        /// </param>
        /// <returns>
        ///   A <see cref="Task"/> that can be <c>await</c>ed to asynchronously create a new implementation of the
        ///   <see cref="IFileSystemWatcher"/> interface to watch the supplied <paramref name="path"/>.
        /// </returns>
        public static Task<IFileSystemWatcher> CreateInstanceAsync(string path, IFileSystemWatcherEventHandler fileWatcherEventHandler = null)
            => new Task<IFileSystemWatcher>(() => new FileSystemWatcherImplementation(path, fileWatcherEventHandler));
    }
}
