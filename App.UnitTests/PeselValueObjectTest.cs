using System;
using ApplicationCore.Models;
using Xunit;

namespace App.UnitTests;

public class PeselValueObjectTest
{
    [Fact]
    public void BirthDate_ShouldDecodeCorrectly()
    {
        var pesel = new Pesel("02252012343");

        Assert.Equal(new DateTime(2002, 5, 20), pesel.BirthDate);
    }

    [Fact]
    public void ControlDigit_ShouldBeCalculatedCorrectly()
    {
        var pesel = new Pesel("02252012343");

        Assert.Equal(3, pesel.ControlDigit);
        Assert.Equal(3, pesel.CalculatedControlDigit);
    }

    [Theory]
    [InlineData("02252012343", Gender.Female)]
    [InlineData("02252012350", Gender.Male)]
    public void Gender_ShouldBeReadCorrectly(string value, Gender expected)
    {
        var pesel = new Pesel(value);

        Assert.Equal(expected, pesel.Gender);
    }

    [Theory]
    [InlineData("02252012343")]
    [InlineData("02252012350")]
    [InlineData("02252012367")]
    [InlineData(" 02252012343 ")] // should trim
    public void Constructor_WithValidPesel_ShouldCreateInstance(string value)
    {
        var pesel = new Pesel(value);
        Assert.Equal(value.Trim(), pesel.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("1234567890")] // too short (10)
    [InlineData("123456789012")] // too long (12)
    [InlineData("0225201234a")] // contains letter
    [InlineData("02252012344")] // invalid checksum
    public void Constructor_WithInvalidPesel_ShouldThrowArgumentException(string value)
    {
        Assert.Throws<ArgumentException>(() => new Pesel(value));
    }

    [Fact]
    public void Equals_WithSameValues_ShouldBeEqual()
    {
        var pesel1 = new Pesel("02252012343");
        var pesel2 = new Pesel("02252012343");

        Assert.Equal(pesel1, pesel2);
        Assert.True(pesel1 == pesel2);
        Assert.False(pesel1 != pesel2);
    }

    [Fact]
    public void Equals_WithDifferentValues_ShouldNotBeEqual()
    {
        var pesel1 = new Pesel("02252012343");
        var pesel2 = new Pesel("02252012350");

        Assert.NotEqual(pesel1, pesel2);
        Assert.False(pesel1 == pesel2);
        Assert.True(pesel1 != pesel2);
    }
}
