using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Slask.Common
{
    public class SystemTime
    {
        internal static ConcurrentDictionary<int, DateTime?> dateTimes;

        public static DateTime Now => dateTimes[Thread.CurrentThread.ManagedThreadId] ?? DateTime.Now;

        public static DateTime Today => dateTimes[Thread.CurrentThread.ManagedThreadId] ?? DateTime.Today;

        public static DateTime UtcNow => dateTimes[Thread.CurrentThread.ManagedThreadId] ?? DateTime.UtcNow;
    }

    public class SystemTimeMocker
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

    ////public class SystemTimeTS
    ////{
    ////    private static DateTime? dateTime;

    ////    public static void Set(DateTime customDateTime) => dateTime = customDateTime;

    ////    public static void Reset() => dateTime = null;

    ////    public static DateTime Now = dateTime ?? DateTime.Now;
    ////}

    ////public class SystemTimeSetterTS
    ////{
    ////    public static 
    ////}


    ////public sealed class DateTimeProvider
    ////{
    ////    private static readonly DateTimeProvider instance = new DateTimeProvider();
    ////    internal ConcurrentDictionary<int, TimeSpan> offsets;

    ////    private DateTimeProvider()
    ////    {
    ////        offsets = new ConcurrentDictionary<int, TimeSpan>();
    ////    }

    ////    public static DateTimeProvider Instance
    ////    {
    ////        get { return instance; }
    ////    }

    ////    public DateTime Now
    ////    {
    ////        get
    ////        {
    ////            int ThreadId = Thread.CurrentThread.ManagedThreadId;

    ////            if (instance.offsets.ContainsKey(ThreadId))
    ////            {
    ////                return DateTime.Now.Add(offsets[ThreadId]);
    ////            }

    ////            return DateTime.Now;
    ////        }
    ////        private set { }
    ////    }
    ////}

    ////public static class DateTimeCreator
    ////{
    ////    public static void SetDateTime(DateTime dateTime)
    ////    {
    ////        int ThreadId = Thread.CurrentThread.ManagedThreadId;
    ////        DateTimeProvider.Instance.offsets[ThreadId] = DateTime.Now.Subtract(dateTime);
    ////    }

    ////    public static void ResetDateTime()
    ////    {
    ////        int ThreadId = Thread.CurrentThread.ManagedThreadId;
    ////        DateTimeProvider.Instance.offsets[ThreadId] = new TimeSpan();
    ////    }
    ////}













    //public class DateTimeProviderTS
    //{
    //    public DateTimeProviderTS()
    //    {
    //        dateTimes = new ConcurrentDictionary<int, DateTime>();

    //    }
    //    //private static DateTimeProvider current = DefaultTimeProvider.Instance;
    //    private static ConcurrentDictionary<int, DateTime> dateTimes;

    //    //public static DateTimeProvider Current
    //    //{
    //    //    get { return DateTimeProvider.current; }
    //    //    set
    //    //    {
    //    //        if (value == null)
    //    //        {
    //    //            throw new ArgumentNullException("value");
    //    //        }
    //    //        DateTimeProvider.current = value;
    //    //    }
    //    //}



    //    public DateTime Now
    //    {
    //        get
    //        {
    //            return dateTimes[Thread.CurrentThread.ManagedThreadId];
    //        }

    //        private set;
    //    }

    //    //public static void ResetToDefault()
    //    //{
    //    //    DateTimeProvider.current = DefaultTimeProvider.Instance;
    //    //}

    //    //public static DateTimeProvider GetDefault()
    //    //{
    //    //    return current;
    //    //}
    //    //private class DefaultTimeProvider : DateTimeProvider
    //    //{
    //    //    private DefaultTimeProvider()
    //    //    {
    //    //    }

    //    //    public static DefaultTimeProvider Instance { get { return Nested.instance; } }

    //    //    private class Nested
    //    //    {
    //    //        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    //    //        static Nested()
    //    //        {
    //    //        }

    //    //        internal static readonly DefaultTimeProvider instance = new DefaultTimeProvider();
    //    //    }

    //    //    public override DateTime Now
    //    //    {
    //    //        get { return DateTime.Now; }
    //    //    }
    //    //}
    //}
    //public abstract class DateTimeProvider
    //{
    //    private static DateTimeProvider current = DefaultTimeProvider.Instance;

    //    //public static DateTimeProvider Current
    //    //{
    //    //    get { return DateTimeProvider.current; }
    //    //    set
    //    //    {
    //    //        if (value == null)
    //    //        {
    //    //            throw new ArgumentNullException("value");
    //    //        }
    //    //        DateTimeProvider.current = value;
    //    //    }
    //    //}

    //    public abstract DateTime Now { get; }

    //    //public static void ResetToDefault()
    //    //{
    //    //    DateTimeProvider.current = DefaultTimeProvider.Instance;
    //    //}

    //    public static DateTimeProvider GetDefault()
    //    {
    //        return current;
    //    }


    //    private class DefaultTimeProvider : DateTimeProvider
    //    {
    //        private DefaultTimeProvider()
    //        {
    //        }

    //        public static DefaultTimeProvider Instance { get { return Nested.instance; } }

    //        private class Nested
    //        {
    //            // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    //            static Nested()
    //            {
    //            }

    //            internal static readonly DefaultTimeProvider instance = new DefaultTimeProvider();
    //        }

    //        public override DateTime Now
    //        {
    //            get { return DateTime.Now; }
    //        }
    //    }
    //}

    //public abstract class DateTimeProvider
    //{
    //    private static DateTimeProvider current = DefaultTimeProvider.Instance;

    //    public static DateTimeProvider Current
    //    {
    //        get { return current; }
    //        set { current = value ?? throw new ArgumentException("value"); }
    //    }

    //    public abstract DateTime Now { get; }

    //    public static void ResetToDefault()
    //    {
    //        current = DefaultTimeProvider.Instance;
    //    }

    //    private sealed class DefaultTimeProvider : DateTimeProvider
    //    {
    //        private DefaultTimeProvider()
    //        {
    //        }

    //        public static DefaultTimeProvider Instance { get { return Nested.instance; } }

    //        private class Nested
    //        {
    //            // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    //            static Nested()
    //            {
    //            }

    //            internal static readonly DefaultTimeProvider instance = new DefaultTimeProvider();
    //        }

    //        public override DateTime Now
    //        {
    //            get { return DateTime.Now; }
    //        }
    //    }
    //}
}
