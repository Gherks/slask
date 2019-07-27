using Moq;
using Slask.Common;
using System;

namespace Slask.TestCore
{
    public class DateTimeMockHelper
    {
        public static void SetTime(DateTime dateTime)
        {
            Mock<DateTimeProvider> timeMock = new Mock<DateTimeProvider>();
            timeMock.SetupGet(dateTimeProvider => dateTimeProvider.Now).Returns(dateTime);
            DateTimeProvider.Current = timeMock.Object;
        }

        public static void ResetTime()
        {
            DateTimeProvider.ResetToDefault();
        }
    }
}
