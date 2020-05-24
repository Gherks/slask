using FluentAssertions;
using Slask.Common;
using System;
using Xunit;

namespace Slask.Xunit.UnitTests.CommonTests
{
    public class SystemTimeTests
    {
        const int acceptableInaccuracy = 2000;
        const int oneDay = 1;

        public SystemTimeTests()
        {
            SystemTimeMocker.Reset();
        }

        [Fact]
        public void SystemTimeNowIsTheSameAsDateTimeNow()
        {
            SystemTime.Now.Should().BeCloseTo(DateTime.Now, acceptableInaccuracy);
        }

        [Fact]
        public void SystemTimeTodayIsTheSameAsDateTimeToday()
        {
            SystemTime.Today.Should().BeCloseTo(DateTime.Today, acceptableInaccuracy);
        }

        [Fact]
        public void SystemTimeUtcNowIsTheSameAsDateTimeUtcNow()
        {
            SystemTime.UtcNow.Should().BeCloseTo(DateTime.UtcNow, acceptableInaccuracy);
        }

        [Fact]
        public void MockedSystemTimeReturnsMockedNow()
        {
            DateTime tomorrow = DateTime.Now.AddDays(oneDay);

            SystemTimeMocker.SetOneSecondAfter(tomorrow);

            SystemTime.Now.Should().BeCloseTo(tomorrow, acceptableInaccuracy);
        }

        [Fact]
        public void MockedSystemTimeReturnsMockedToday()
        {
            SystemTimeMocker.SetOneSecondAfter(DateTime.Now.AddDays(oneDay));

            SystemTime.Today.Should().BeCloseTo(DateTime.Today.AddDays(oneDay), acceptableInaccuracy);
        }

        [Fact]
        public void MockedSystemTimeReturnsMockedUtcNow()
        {
            SystemTimeMocker.SetOneSecondAfter(DateTime.Now.AddDays(oneDay));

            SystemTime.UtcNow.Should().BeCloseTo(DateTime.UtcNow.AddDays(oneDay), acceptableInaccuracy);
        }

        [Fact]
        public void SystemTimeCanBeResetAfterBeingMocked()
        {
            SystemTimeMocker.SetOneSecondAfter(DateTime.Now.AddDays(oneDay));
            SystemTimeMocker.Reset();

            SystemTime.Now.Should().BeCloseTo(DateTime.Now, acceptableInaccuracy);
        }
    }
}
