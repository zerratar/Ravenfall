using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Shinobytes.Ravenfall.RavenNet.Core
{
    public class Kernel : IKernel
    {
        private readonly object mutex = new object();
        private readonly object timeoutMutex = new object();
        private readonly CancellationTokenSource cancellationTokenSource;
        private List<TimeoutCallbackHandle> timeouts = new List<TimeoutCallbackHandle>();
        private List<TickCallbackHandle> ticks = new List<TickCallbackHandle>();

        private Thread kernelThread;
        private bool started;
        private bool disposed;

        public Kernel()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
        }

        public ITimeoutHandle SetTimeout(Action action, int timeoutMilliseconds)
        {
            lock (mutex)
            {
                if (!this.started)
                {
                    Start();
                }
            }
            lock (timeoutMutex)
            {
                var timeout = new TimeoutCallbackHandle(action, timeoutMilliseconds);
                this.timeouts.Add(timeout);
                this.timeouts = this.timeouts.OrderBy(x => x.Timeout).ToList();
                return timeout;
            }
        }

        public void ClearTimeout(ITimeoutHandle handle)
        {
            lock (timeoutMutex)
            {
                this.timeouts.Remove((TimeoutCallbackHandle)handle);
            }
        }

        public void Start()
        {
            lock (mutex)
            {
                if (this.started)
                {
                    return;
                }

                this.started = true;
                this.kernelThread = new Thread(KernelProcess)
                {
                    Name = "Kernel",
                    IsBackground = true
                };
                this.kernelThread.Start();
            }
        }

        public void Stop()
        {
            lock (mutex)
            {
                this.started = false;
                this.kernelThread.Join();
            }
        }

        public bool Started => started;

        private void KernelProcess()
        {
            do
            {
                lock (mutex)
                {
                    if (!this.started)
                    {
                        return;
                    }
                }

                var timeout = 100;
                try
                {
                    lock (timeoutMutex)
                    {
                        foreach (var tick in ticks)
                        {
                            timeout = Math.Min(timeout, (int)tick.Timeout.TotalMilliseconds);
                            tick.Invoke();
                        }

                        var item = this.timeouts.OrderBy(x => x.Timeout).FirstOrDefault();
                        if (item != null)
                        {
                            if (DateTime.Now >= item.Registered.Add(item.Timeout))
                            {
                                ClearTimeout(item);
                                item.Action?.Invoke();
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    // ignored, we can't have the kernel die due to an exception
                }

                System.Threading.Thread.Sleep(timeout);
            } while (!cancellationTokenSource.IsCancellationRequested);
        }

        public void Dispose()
        {
            if (disposed) return;

            lock (timeoutMutex)
            {
                this.timeouts.Clear();
            }
            cancellationTokenSource.Cancel();
            kernelThread.Join();
            disposed = true;
        }

        public void RegisterTickUpdate(Action<TimeSpan> update, TimeSpan timeSpan)
        {
            lock (mutex)
            {
                if (!this.started)
                {
                    Start();
                }
            }

            lock (timeoutMutex)
            {
                ticks.Add(new TickCallbackHandle(update, timeSpan));
            }
        }

        private class TimeoutCallbackHandle : ITimeoutHandle
        {
            public TimeoutCallbackHandle(Action action, int timeout)
            {
                this.Registered = DateTime.Now;
                this.Timeout = TimeSpan.FromMilliseconds(timeout);
                this.Action = action;
            }

            public DateTime Registered { get; }
            public TimeSpan Timeout { get; }
            public Action Action { get; }
            public Guid Id { get; } = Guid.NewGuid();
        }

        private class TickCallbackHandle : ITimeoutHandle
        {
            private DateTime lastInvoke;

            public TickCallbackHandle(Action<TimeSpan> action, TimeSpan timeout)
            {
                this.Timeout = timeout;
                this.Action = action;
                this.lastInvoke = DateTime.UtcNow;
            }

            public TimeSpan Timeout { get; }
            public Action<TimeSpan> Action { get; }
            public Guid Id { get; } = Guid.NewGuid();

            internal void Invoke()
            {
                var now = DateTime.UtcNow;
                var delta = now - lastInvoke;
                Action?.Invoke(delta);
                lastInvoke = now;
            }
        }
    }
}
