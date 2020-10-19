using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Slask.Common.Xunit.UnitTests
{
    public class StringExtensionTests
    {
        [Fact]
        public void CanCreateStringListFromStringWithCommaDelimiter()
        {
            string text = "Zero,One,Two,Three";

            List<string> stringList = text.ToStringList(",");

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

            List<string> stringList = text.ToStringList("|");

            stringList.Should().HaveCount(3);
            stringList[0].Should().Be("eagle");
            stringList[1].Should().Be("bear");
            stringList[2].Should().Be("cow");
        }

        [Fact]
        public void CanCreateStringListFromStringWithCharacterDelimiter()
        {
            string text = "4a3a2a1";

            List<string> stringList = text.ToStringList("a");

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

            List<string> stringList = text.ToStringList("1");

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

            List<string> stringList = text.ToStringList("_");

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

            List<string> stringList = text.ToStringList("abc");

            stringList.Should().HaveCount(3);
            stringList[0].Should().Be("1");
            stringList[1].Should().Be("cat");
            stringList[2].Should().Be("lol");
        }

        [Fact]
        public void CanCreateStringListFromStringStartingWithDelimiter()
        {
            string text = ",omega,yikes";

            List<string> stringList = text.ToStringList(",");

            stringList.Should().HaveCount(2);
            stringList[0].Should().Be("omega");
            stringList[1].Should().Be("yikes");
        }

        [Fact]
        public void CanCreateStringListFromStringWithTrailingDelimiter()
        {
            string text = "kaktus,galaxus,";

            List<string> stringList = text.ToStringList(",");

            stringList.Should().HaveCount(2);
            stringList[0].Should().Be("kaktus");
            stringList[1].Should().Be("galaxus");
        }

        [Fact]
        public void RemovesWhitespaceFromStringElements()
        {
            string text = "  Zero, One ,Two ,  Three    ";

            List<string> stringList = text.ToStringList(",");

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

            List<string> stringList = text.ToStringList(" ");

            stringList.Should().BeEmpty();
        }

        [Fact]
        public void CannotCreateStringListWithEmptyDelimiter()
        {
            string text = "Never|stop|never|stopping";

            List<string> stringList = text.ToStringList("");

            stringList.Should().BeEmpty();
        }

        [Fact]
        public void CanTransformStringToUppserWithNoSpaces()
        {
            string transformedText = "to upper no spaces".ToUpperNoSpaces();

            transformedText.Should().Be("TOUPPERNOSPACES");
        }

        [Fact]
        public void TransformingEmptyStringToUpperNoSpacesReturnsUntouchedString()
        {
            string transformedText = "".ToUpperNoSpaces();

            transformedText.Should().Be("");
        }

        [Fact]
        public void TransformingNullStringToUpperNoSpacesReturnsNull()
        {
            string transformedText = StringExtensions.ToUpperNoSpaces(null);

            transformedText.Should().BeNull();
        }
    }
}
