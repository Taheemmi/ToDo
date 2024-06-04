using Xunit;

namespace ToDoList.Tests
{
    public class UserAuthTests
    {
        [Theory]
        [InlineData("user", "root", true)]
        [InlineData("user", "wrongpassword", false)]
        [InlineData("wronguser", "root", false)]
        [InlineData("wronguser", "wrongpassword", false)]
        [InlineData("user", " root", false)]
        [InlineData(" user", "root", false)]
        public void Authenticate_ShouldReturnExpectedResult(string inputUsername, string inputPassword, bool expected)
        {
            // Act
            bool result = UserAuth.Authenticate(inputUsername, inputPassword);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
