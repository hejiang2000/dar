using System;
using System.Threading;

namespace DAR.Internal
{
    internal sealed class Lock : IDisposable
    {
        private ReaderWriterLockSlim _thisLock =
            new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private int _isDisposed = 0;
        public void EnterReadLock()
        {
            this._thisLock.EnterReadLock();
        }

        public void EnterWriteLock()
        {
            this._thisLock.EnterWriteLock();
        }

        public void ExitReadLock()
        {
            this._thisLock.ExitReadLock();
        }

        public void ExitWriteLock()
        {
            this._thisLock.ExitWriteLock();
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref this._isDisposed, 1, 0) == 0)
            {
                this._thisLock.Dispose();
            }
        }
    }

    internal struct ReadLock : IDisposable
    {
        private readonly Lock _lock;
        private int _isDisposed;

        public ReadLock(Lock @lock)
        {
            this._isDisposed = 0;
            this._lock = @lock;
            this._lock.EnterReadLock();
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref this._isDisposed, 1, 0) == 0)
            {
                this._lock.ExitReadLock();
            }
        }
    }

    internal struct WriteLock : IDisposable
    {
        private readonly Lock _lock;
        private int _isDisposed;

        public WriteLock(Lock @lock)
        {
            this._isDisposed = 0;
            this._lock = @lock;
            this._lock.EnterWriteLock();
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref this._isDisposed, 1, 0) == 0)
            {
                this._lock.ExitWriteLock();
            }
        }
    }
}
