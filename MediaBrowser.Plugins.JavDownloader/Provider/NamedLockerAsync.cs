// -----------------------------------------------------------------------
// <copyright file="NamedLockerAsync.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Provider
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// 命名锁.
    /// </summary>
    public class NamedLockerAsync
    {
        /// <summary>
        /// Defines the _lockDict.
        /// </summary>
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _lockDict = new ConcurrentDictionary<string, SemaphoreSlim>();

        /// <summary>
        /// 获取锁对象.
        /// </summary>
        /// <param name="name">.</param>
        /// <returns>.</returns>
        public SemaphoreSlim GetLock(string name)
            => _lockDict.GetOrAdd(name, s => new SemaphoreSlim(1, 1));

        /// <summary>
        /// 执行带返回结果的锁定.
        /// </summary>
        /// <typeparam name="TResult">返回结果.</typeparam>
        /// <param name="name">锁名.</param>
        /// <param name="func">执行方法.</param>
        /// <param name="auto_remove">自动移除锁对象.</param>
        /// <returns>.</returns>
        public async Task<TResult> RunWithLock<TResult>(string name, Func<Task<TResult>> func, bool auto_remove = true)
        {
            var locker = _lockDict.GetOrAdd(name, s => new SemaphoreSlim(1, 1));
            await locker.WaitAsync();
            try
            {
                var t = await func();
                return t;
            }
            finally
            {
                locker.Release();
                Thread.Sleep(1);
                if (locker.CurrentCount == 1)
                {
                    _lockDict.TryRemove(name, out locker);
                    if (locker.CurrentCount == 1)
                        locker.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行带返回结果的锁定.
        /// </summary>
        /// <param name="name">锁名.</param>
        /// <param name="action">The action.</param>
        /// <param name="auto_remove">自动移除锁对象.</param>
        /// <returns>.</returns>
        public async Task RunWithLock(string name, Func<Task> action, bool auto_remove = true)
        {
            var locker = _lockDict.GetOrAdd(name, s => new SemaphoreSlim(1, 1));
            await locker.WaitAsync();
            try
            {
                await action();
            }
            finally
            {
                locker.Release();
                Thread.Sleep(1);
                if (locker.CurrentCount == 1)
                {
                    _lockDict.TryRemove(name, out locker);
                    if (locker.CurrentCount == 1)
                        locker.Dispose();
                }
            }
        }

        /// <summary>
        /// The RemoveLock.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        public void RemoveLock(string name)
        {
            SemaphoreSlim o;
            _lockDict.TryRemove(name, out o);
        }

        /// <summary>
        /// The LockAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IDisposable}"/>.</returns>
        public Task<IDisposable> LockAsync(string name)
        {
            var m_semaphore = GetLock(name);
            var wait = m_semaphore.WaitAsync();
            var m_releaser = Task.FromResult((IDisposable)new Releaser(this, name, m_semaphore));
            return wait.IsCompleted ?
                        m_releaser :
                        wait.ContinueWith((_, state) => (IDisposable)state,
                            m_releaser.Result, CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        /// <summary>
        /// Defines the <see cref="Releaser" />.
        /// </summary>
        private sealed class Releaser : IDisposable
        {
            /// <summary>
            /// Defines the named_locker.
            /// </summary>
            private readonly NamedLockerAsync named_locker;

            /// <summary>
            /// Defines the name.
            /// </summary>
            private readonly string name;

            /// <summary>
            /// Defines the m_semaphore.
            /// </summary>
            private readonly SemaphoreSlim m_semaphore;

            /// <summary>
            /// Initializes a new instance of the <see cref="Releaser"/> class.
            /// </summary>
            /// <param name="named_locker">The named_locker<see cref="NamedLockerAsync"/>.</param>
            /// <param name="name">The name<see cref="string"/>.</param>
            /// <param name="m_semaphore">The m_semaphore<see cref="SemaphoreSlim"/>.</param>
            internal Releaser(NamedLockerAsync named_locker, string name, SemaphoreSlim m_semaphore)
            {
                this.named_locker = named_locker;
                this.name = name;
                this.m_semaphore = m_semaphore;
            }

            /// <summary>
            /// The Dispose.
            /// </summary>
            public void Dispose()
            {
                m_semaphore.Release();
                Thread.Sleep(1);
                if (m_semaphore.CurrentCount == 1)
                {
                    named_locker.RemoveLock(name);
                    if (m_semaphore.CurrentCount == 1)
                        m_semaphore.Dispose();
                }
            }
        }
    }
}
