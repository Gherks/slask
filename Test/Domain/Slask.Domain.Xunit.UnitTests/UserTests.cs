using FluentAssertions;
using Xunit;

namespace Slask.Domain.Xunit.UnitTests
{
    public class UserTests
    {
        [Fact]
        public void CanCreateUser()
        {
            User user = User.Create("Stålberto");

            user.Should().NotBeNull();
            user.Id.Should().NotBeEmpty();
            user.Name.Should().Be("Stålberto");
        }

        [Fact]
        public void CannotCreateUserWithEmptyName()
        {
            User user = User.Create("");

            user.Should().BeNull();
        }
    }
}
