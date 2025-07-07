using Xunit;
using FluentAssertions;
using System;
using System.Collections.Generic;

namespace Charity_BE.Tests.SimpleTests
{
    public class BasicTests
    {
        [Fact]
        public void BasicTest_ShouldPass()
        {
            // Arrange
            var expected = 2;
            var actual = 1 + 1;

            // Act & Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void StringTest_ShouldPass()
        {
            // Arrange
            var expected = "Hello World";
            var actual = "Hello " + "World";

            // Act & Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(5, 5, 10)]
        [InlineData(-1, 1, 0)]
        public void AdditionTest_ShouldPass(int a, int b, int expected)
        {
            // Act
            var actual = a + b;

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void ListTest_ShouldPass()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };

            // Act & Assert
            list.Should().HaveCount(5);
            list.Should().Contain(3);
            list.Should().NotContain(6);
        }

        [Fact]
        public void DateTimeTest_ShouldPass()
        {
            // Arrange
            var now = DateTime.Now;

            // Act & Assert
            now.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }
    }
} 