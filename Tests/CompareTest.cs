using System.Reflection;
using XmlCompare.Model;
using XmlCompare.Presenter;
using XmlCompare.View;
using Xunit;

namespace XmlCompare.Tests
{
    [Collection("Our Test Collection")]
    public class CompareTest
    {
        [Theory]
        [InlineData(@"../../Tests/xml/test.xml", @"../../Tests/xml/test.xml")]
        [InlineData(@"../../Tests/xml/empty.xml", @"../../Tests/xml/empty.xml")]
        void CompareForEqual(string f1, string f2)
        {
            var mf = new MainForm(true);
            var settings = Settings.GetSettings();
            settings.LeftFileName = f1;
            settings.RightFileName = f2;
            settings.IsShowDifferences = true;
            var sp = new SettingsPresenter(mf, settings);
            var cp = new ComparePresenter(mf, sp, null);
            // invoke comparing
            MethodInfo dynMethod = mf.GetType().GetMethod("compareAgainToolStripMenuItem_Click",
                BindingFlags.NonPublic | BindingFlags.Instance);
            //dynMethod.Invoke(mf, new object[] { null, null });
        }

        [Theory]
        [InlineData(@"../../Tests/xml/oneelement.xml", @"../../Tests/xml/corrupted.xml")]
        [InlineData(@"../../Tests/xml/corrupted.xml", @"../../Tests/xml/oneelement.xml")]
        [InlineData("absent.xml", @"../../Tests/xml/oneelement.xml")]
        [InlineData(@"../../Tests/xml/oneelement.xml", "absent.xml")]
        [InlineData(@"../../Tests/xml/empty.xml", "../../Tests/xml/oneelement.xml")]
        [InlineData(@"../../Tests/xml/oneelement.xml", "../../Tests/xml/empty.xml")]
        [InlineData(@"../../Tests/xml/onlyheader.xml", "../../Tests/xml/oneelement.xml")]
        [InlineData(@"../../Tests/xml/oneelement.xml", "../../Tests/xml/onlyheader.xml")]
        [InlineData(@"../../Tests/xml/onlyheader.xml", "../../Tests/xml/onlyheader.xml")]
        void CompareForCorrupted(string f1, string f2)
        {
            var mf = new MainForm(true);
            var settings = Settings.GetSettings();
            settings.LeftFileName = f1;
            settings.RightFileName = f2;
            settings.IsShowDifferences = true;
            var sp = new SettingsPresenter(mf, settings);
            var cp = new ComparePresenter(mf, sp, null);
            // invoke comparing
            MethodInfo dynMethod = cp.GetType().GetMethod("OnSettingsChanged",
                BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(cp, new object[] { settings, true });
            Assert.False(cp.CompareResult.IsSuccessed);
        }

        [Theory]
        [InlineData(@"../../Tests/xml/oneelement.xml", "../../Tests/xml/oneelement.xml", true)]
        [InlineData(@"../../Tests/xml/oneelement.xml", "../../Tests/xml/twoelements.xml", false)]
        [InlineData(@"../../Tests/xml/twoelements.xml", "../../Tests/xml/oneelement.xml", false)]
        [InlineData(@"../../Tests/xml/oneelement.xml", "../../Tests/xml/twoelements01.xml", false)]
        [InlineData(@"../../Tests/xml/twoelements01.xml", "../../Tests/xml/oneelement.xml", false)]
        [InlineData(@"../../Tests/xml/twoelements01.xml", "../../Tests/xml/threeelements01.xml", false)]
        [InlineData(@"../../Tests/xml/threeelements01.xml", "../../Tests/xml/twoelements01.xml", false)]
        void CompareSuccessed(string f1, string f2, bool isequal)
        {
            var mf = new MainForm(true);
            var settings = Settings.GetSettings();
            settings.LeftFileName = f1;
            settings.RightFileName = f2;
            settings.IsShowDifferences = true;
            var sp = new SettingsPresenter(mf, settings);
            var cp = new ComparePresenter(mf, sp, null);
            // invoke comparing
            MethodInfo dynMethod = cp.GetType().GetMethod("OnSettingsChanged",
                BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(cp, new object[] { settings, true });
            Assert.True(cp.CompareResult.IsSuccessed);
            Assert.True(isequal? !cp.CompareResult.HasDifferences : cp.CompareResult.HasDifferences);
        }

    }
}
