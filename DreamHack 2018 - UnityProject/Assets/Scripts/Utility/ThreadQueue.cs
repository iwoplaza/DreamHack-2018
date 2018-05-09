using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Game.Utility
{
    public class ThreadQueue<T>
    {
        private readonly Queue<T> m_Queue;
        private readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();

        public ThreadQueue()
        {
            m_Queue = new Queue<T>();
        }

        public ThreadQueue(int capacity)
        {
            m_Queue = new Queue<T>(capacity);
        }

        public ThreadQueue(IEnumerable<T> collection)
        {
            m_Queue = new Queue<T>(collection);
        }

        public IEnumerator<T> GetEnumerator()
        {
            Queue<T> localQ;

            Lock.EnterReadLock();
            try
            {
                localQ = new Queue<T>(m_Queue);
            }
            finally
            {
                Lock.ExitReadLock();
            }

            foreach (T item in localQ)
                yield return item;
        }

        public void Enqueue(T item)
        {
            Lock.EnterWriteLock();
            try
            {
                m_Queue.Enqueue(item);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public T Dequeue()
        {
            Lock.EnterWriteLock();
            try
            {
                return m_Queue.Dequeue();
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public bool TryDequeue(out T result)
        {
            result = default(T);
            Lock.EnterWriteLock();
            try
            {
                if (m_Queue.Count > 0)
                {
                    result = m_Queue.Dequeue();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public int Count
        {
            get
            {
                int count = 0;
                Lock.EnterReadLock();
                try
                {
                    count = m_Queue.Count;
                }
                finally
                {
                    Lock.ExitReadLock();
                }
                return count;
            }
        }
    }
}