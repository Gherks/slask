using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Slask.Common
{
    public static class SystemTime
    {
        private static int concurrencyLevel = Environment.ProcessorCount * 2;

        internal static ConcurrentDictionary<int, DateTime?> dateTimes = new ConcurrentDictionary<int, DateTime?>(concurrencyLevel, concurrencyLevel);

        public static DateTime Now
        {
            get
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;

                if (!dateTimes.Keys.Contains(threadId))
                {
                    dateTimes[threadId] = null;
                }
                else if (dateTimes[threadId] != null)
                {
                    return dateTimes[threadId].Value;
                }

                return DateTime.Now;
            }

            private set { }
        }

        public static DateTime Today
        {
            get
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;

                if (!dateTimes.Keys.Contains(threadId))
                {
                    dateTimes[threadId] = null;
                }
                else if (dateTimes[threadId] != null)
                {
                    return dateTimes[threadId].Value;
                }

                return DateTime.Today;
            }

            private set { }
        }

        public static DateTime UtcNow
        {
            get
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;

                if (!dateTimes.Keys.Contains(threadId))
                {
                    dateTimes[threadId] = null;
                }
                else if (dateTimes[threadId] != null)
                {
                    return dateTimes[threadId].Value;
                }

                return DateTime.UtcNow;
            }

            private set { }
        }
    }

    public static class SystemTimeMocker
    {
        public static void Set(DateTime mockedDateTime)
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            SystemTime.dateTimes[ThreadId] = mockedDateTime;
        }

        public static void Reset()
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            SystemTime.dateTimes[ThreadId] = null;
        }
    }
}