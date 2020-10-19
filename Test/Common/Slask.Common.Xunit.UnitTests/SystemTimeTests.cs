using FluentAssertions;
using System;
using Xunit;

namespace Slask.Common.Xunit.UnitTests
{
    public class SystemTimeTests
    {
        private const int _acceptableInaccuracy = 2000;
        private const int _oneDay = 1;

        public SystemTimeTests()
        {
            SystemTimeMocker.Reset();
        }

        [Fact]
        public void SystemTimeNowIsTheSameAsDateTimeNow()
        {
            SystemTime.Now.Should().BeCloseTo(DateTime.Now, _acceptableInaccuracy);
        }

        [Fact]
        public void SystemTimeTodayIsTheSameAsDateTimeToday()
        {
            SystemTime.Today.Should().BeCloseTo(DateTime.Today, _acceptableInaccuracy);
        }

        [Fact]
        public void SystemTimeUtcNowIsTheSameAsDateTimeUtcNow()
        {
            SystemTime.UtcNow.Should().BeCloseTo(DateTime.UtcNow, _acceptableInaccuracy);
        }

        [Fact]
        public void MockedSystemTimeReturnsMockedNow()
        {
            DateTime tomorrow = DateTime.Now.AddDays(_oneDay);

            SystemTimeMocker.SetOneSecondAfter(tomorrow);

            SystemTime.Now.Should().BeCloseTo(tomorrow, _acceptableInaccuracy);
        }

        [Fact]
        public void MockedSystemTimeReturnsMockedToday()
        {
            SystemTimeMocker.SetOneSecondAfter(DateTime.Now.AddDays(_oneDay));

            SystemTime.Today.Should().BeCloseTo(DateTime.Today.AddDays(_oneDay), _acceptableInaccuracy);
        }

        [Fact]
        public void MockedSystemTimeReturnsMockedUtcNow()
        {
            SystemTimeMocker.SetOneSecondAfter(DateTime.Now.AddDays(_oneDay));

            SystemTime.UtcNow.Should().BeCloseTo(DateTime.UtcNow.AddDays(_oneDay), _acceptableInaccuracy);
        }

        [Fact]
        public void SystemTimeCanBeResetAfterBeingMocked()
        {
            SystemTimeMocker.SetOneSecondAfter(DateTime.Now.AddDays(_oneDay));
            SystemTimeMocker.Reset();

            SystemTime.Now.Should().BeCloseTo(DateTime.Now, _acceptableInaccuracy);
        }
    }
}
