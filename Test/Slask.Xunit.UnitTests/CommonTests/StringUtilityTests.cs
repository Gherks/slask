using FluentAssertions;
using Slask.Common;
using System.Collections.Generic;
using Xunit;

namespace Slask.UnitTests.CommonTests
{
    public class StringUtilityTests
    {
        [Fact]
        public void CanCreateStringListFromStringWithCommaDelimiter()
        {
            string text = "Zero,One,Two,Three";

            List<string> stringList = StringUtility.ToStringList(text, ",");

            stringList.Should().HaveCount(4);
            stringList[0].Should().Be("Zero");
            stringList[1].Should().Be("One");
            stringList[2].Should().Be("Two");
            stringList[3].Should().Be("Three");
        }

        [Fact]
        public void CanCreateStringListFromStringWithPipeDelimiter()
        {
            string text = "eagle|bear|cow";

            List<string> stringList = StringUtility.ToStringList(text, "|");

            stringList.Should().HaveCount(3);
            stringList[0].Should().Be("eagle");
            stringList[1].Should().Be("bear");
            stringList[2].Should().Be("cow");
        }

        [Fact]
        public void CanCreateStringListFromStringWithCharacterDelimiter()
        {
            string text = "4a3a2a1";

            List<string> stringList = StringUtility.ToStringList(text, "a");

            stringList.Should().HaveCount(4);
            stringList[0].Should().Be("4");
            stringList[1].Should().Be("3");
            stringList[2].Should().Be("2");
            stringList[3].Should().Be("1");
        }

        [Fact]
        public void CanCreateStringListFromStringWithStringNumberDelimiter()
        {
            string text = "cheese1cheese1cheese1cheese";

            List<string> stringList = StringUtility.ToStringList(text, "1");

            stringList.Should().HaveCount(4);
            stringList[0].Should().Be("cheese");
            stringList[1].Should().Be("cheese");
            stringList[2].Should().Be("cheese");
            stringList[3].Should().Be("cheese");
        }

        [Fact]
        public void CanCreateStringListFromStringWithUnderscoreDelimiter()
        {
            string text = "T_O_P_K_E_K";

            List<string> stringList = StringUtility.ToStringList(text, "_");

            stringList.Should().HaveCount(6);
            stringList[0].Should().Be("T");
            stringList[1].Should().Be("O");
            stringList[2].Should().Be("P");
            stringList[3].Should().Be("K");
            stringList[4].Should().Be("E");
            stringList[5].Should().Be("K");
        }

        [Fact]
        public void CanCreateStringListFromStringWithMultiCharacteredDelimiter()
        {
            string text = "1abccatabclol";

            List<string> stringList = StringUtility.ToStringList(text, "abc");

            stringList.Should().HaveCount(3);
            stringList[0].Should().Be("1");
            stringList[1].Should().Be("cat");
            stringList[2].Should().Be("lol");
        }

        [Fact]
        public void CanCreateStringListFromStringStartingWithDelimiter()
        {
            string text = ",omega,yikes";

            List<string> stringList = StringUtility.ToStringList(text, ",");

            stringList.Should().HaveCount(2);
            stringList[0].Should().Be("omega");
            stringList[1].Should().Be("yikes");
        }

        [Fact]
        public void CanCreateStringListFromStringWithTrailingDelimiter()
        {
            string text = "kaktus,galaxus,";

            List<string> stringList = StringUtility.ToStringList(text, ",");

            stringList.Should().HaveCount(2);
            stringList[0].Should().Be("kaktus");
            stringList[1].Should().Be("galaxus");
        }

        [Fact]
        public void RemovesWhitespaceFromStringElements()
        {
            string text = "  Zero, One ,Two ,  Three    ";

            List<string> stringList = StringUtility.ToStringList(text, ",");

            stringList.Should().HaveCount(4);
            stringList[0].Should().Be("Zero");
            stringList[1].Should().Be("One");
            stringList[2].Should().Be("Two");
            stringList[3].Should().Be("Three");
        }

        [Fact]
        public void CannotCreateStringListWithSpaceDelimiter()
        {
            string text = "Woof Woof Woof Woof";

            List<string> stringList = StringUtility.ToStringList(text, " ");

            stringList.Should().BeEmpty();
        }

        [Fact]
        public void CannotCreateStringListWithEmptyDelimiter()
        {
            string text = "Never|stop|never|stopping";

            List<string> stringList = StringUtility.ToStringList(text, "");

            stringList.Should().BeEmpty();
        }

        [Fact]
        public void CanTransformStringToUppserWithNoSpaces()
        {
            string transformedText = StringUtility.ToUpperNoSpaces("to upper no spaces");

            transformedText.Should().Be("TOUPPERNOSPACES");
        }

        [Fact]
        public void TransformingEmptyStringToUpperNoSpacesReturnsUntouchedString()
        {
            string transformedText = StringUtility.ToUpperNoSpaces("");

            transformedText.Should().Be("");
        }

        [Fact]
        public void TransformingNullStringToUpperNoSpacesReturnsNull()
        {
            string transformedText = StringUtility.ToUpperNoSpaces(null);

            transformedText.Should().Be(null);
        }

        [Fact]
        public void CheckingNullStringWithIsNullOrEmptyReturnsTrue()
        {
            StringUtility.IsNullOrEmpty(null).Should().BeTrue();
        }

        [Fact]
        public void CheckingEmptyStringWithIsNullOrEmptyReturnsTrue()
        {
            StringUtility.IsNullOrEmpty("").Should().BeTrue();
        }

        [Fact]
        public void CheckingWhitespaceStringWithIsNullOrEmptyReturnsTrue()
        {
            StringUtility.IsNullOrEmpty("  ").Should().BeTrue();
        }

        [Fact]
        public void CheckingValidStringWithIsNullOrEmptyReturnsFalse()
        {
            StringUtility.IsNullOrEmpty("lol").Should().BeFalse();
        }
    }
}
