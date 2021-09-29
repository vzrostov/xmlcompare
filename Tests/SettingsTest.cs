using Xunit;

namespace XmlCompare.Tests
{
    public class SettingsTest
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(13, Add());
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(4, Add2());
        }

        int Add() => 4 + 9;
        int Add2() => 2 + 2;
    }
}
