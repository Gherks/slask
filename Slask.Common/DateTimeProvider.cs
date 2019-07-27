using System;

namespace Slask.Common
{
    public abstract class DateTimeProvider
    {
        private static DateTimeProvider current = DefaultTimeProvider.Instance;

        public static DateTimeProvider Current
        {
            get { return current; }
            set { current = value ?? throw new ArgumentException("value"); }
        }

        public abstract DateTime Now { get; }

        public static void ResetToDefault()
        {
            current = DefaultTimeProvider.Instance;
        }

        private sealed class DefaultTimeProvider : DateTimeProvider
        {
            private DefaultTimeProvider()
            {
            }

            public static DefaultTimeProvider Instance { get { return Nested.instance; } }

            private class Nested
            {
                // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
                static Nested()
                {
                }

                internal static readonly DefaultTimeProvider instance = new DefaultTimeProvider();
            }

            public override DateTime Now
            {
                get { return DateTime.Now; }
            }
        }
    }
}
