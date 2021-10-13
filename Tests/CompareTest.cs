using System.Reflection;
using System.Linq;
using XmlCompare.Model;
using XmlCompare.Presenter;
using XmlCompare.View;
using Xunit;
using System.Diagnostics;

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
        [InlineData(@"../../Tests/xml/tworoots.xml", "../../Tests/xml/root1.xml")]
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

        /// <summary>
        /// Check successed comparing
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="isequal">Two files are equal</param>
        /// <param name="metrics">Count of nodes with definite mode. Size 9 - only diffmodes, size 11 - + DiffInside modes</param>
        [Theory]
        [InlineData(@"../../Tests/xml/oneelement.xml", "../../Tests/xml/oneelement.xml", true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
        [InlineData(@"../../Tests/xml/oneelement.xml", "../../Tests/xml/twoelements.xml", false, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0)]
        [InlineData(@"../../Tests/xml/twoelements.xml", "../../Tests/xml/oneelement.xml", false, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0)]
        [InlineData(@"../../Tests/xml/oneelement.xml", "../../Tests/xml/twoelements01.xml", false, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1)]
        [InlineData(@"../../Tests/xml/twoelements01.xml", "../../Tests/xml/oneelement.xml", false, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 1)]
        [InlineData(@"../../Tests/xml/twoelements01.xml", "../../Tests/xml/threeelements012.xml", false, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0)]
        [InlineData(@"../../Tests/xml/threeelements012.xml", "../../Tests/xml/twoelements01.xml", false, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0)]
        [InlineData(@"../../Tests/xml/threeelements012-034.xml", "../../Tests/xml/threeelements012-012.xml", false, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, 2)]
        [InlineData(@"../../Tests/xml/threeelements012-012.xml", "../../Tests/xml/threeelements012-034.xml", false, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, 2)]
        [InlineData(@"../../Tests/xml/twoelementsTextInside.xml", "../../Tests/xml/twoelementsTextInsideEq.xml", false, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0)]
        [InlineData(@"../../Tests/xml/twoelementsTextInsideEq.xml", "../../Tests/xml/twoelementsTextInside.xml", false, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0)]
        [InlineData(@"../../Tests/xml/twoelementsTextInside.xml", "../../Tests/xml/twoelementsTextInsideNEq.xml", false, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0)]
        [InlineData(@"../../Tests/xml/twoelementsTextInsideNEq.xml", "../../Tests/xml/twoelementsTextInside.xml", false, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0)]
        [InlineData(@"../../Tests/xml/twoelementsTextInsideRoot.xml", "../../Tests/xml/twoelementsTextInsideRoot2.xml", false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0)]
        [InlineData(@"../../Tests/xml/twoelementsTextInsideRoot2.xml", "../../Tests/xml/twoelementsTextInsideRoot.xml", false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0)]
        [InlineData(@"../../Tests/xml/hierarchy1.xml", "../../Tests/xml/hierarchy2.xml", false, 4, 2, 1, 1, 0, 1, 0, 0, 0, 0, 3, 2)]
        [InlineData(@"../../Tests/xml/hierarchy2.xml", "../../Tests/xml/hierarchy1.xml", false, 4, 1, 2, 1, 1, 0, 0, 0, 0, 0, 3, 2)]
        [InlineData(@"../../Tests/xml/hierarchy1.xml", "../../Tests/xml/hierarchyroot.xml", false, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1)]
        [InlineData(@"../../Tests/xml/hierarchyroot.xml", "../../Tests/xml/hierarchy1.xml", false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1)]
        [InlineData(@"../../Tests/xml/root1.xml", "../../Tests/xml/root2.xml", true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
        [InlineData(@"../../Tests/xml/root2.xml", "../../Tests/xml/root1.xml", true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
        void CompareSuccessed(string f1, string f2, bool isequal, params int[] metrics)
        {
            var mf = new MainForm(true);
            var settings = Settings.GetSettings();
            settings.LeftFileName = f1;
            settings.RightFileName = f2;
            settings.IsShowDifferences = true;
            var sp = new SettingsPresenter(mf, settings);
            var cp = new ComparePresenter(mf, sp, null);
            // invoke comparing
            MethodInfo dynMethod = cp.GetType().GetMethod("OnSettingsChanged", BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(cp, new object[] { settings, true });
            Assert.True(cp.CompareResult.IsSuccessed);
            Assert.True(isequal? !cp.CompareResult.HasDifferences : cp.CompareResult.HasDifferences);
            // checking
            Assert.True(metrics.Length==10 || metrics.Length == 12);
            var ее = cp.CompareResult.Data.Flatten();
            int[] metricsAfter = { 
                cp.CompareResult.Data.FilterByMode(new[] { NodeMode.ElementChanged }).Count(),
                cp.CompareResult.Data.FilterByMode(new[] { NodeMode.ElementAdded }).Count(),
                cp.CompareResult.Data.FilterByMode(new[] { NodeMode.ElementRemoved }).Count(),
                cp.CompareResult.Data.FilterByMode(new[] { NodeMode.AttributeChanged }).Count(),
                cp.CompareResult.Data.FilterByMode(new[] { NodeMode.AttributeAdded }).Count(),
                cp.CompareResult.Data.FilterByMode(new[] { NodeMode.AttributeRemoved }).Count(),
                cp.CompareResult.Data.FilterByMode(new[] { NodeMode.CommentChanged }).Count(),
                cp.CompareResult.Data.FilterByMode(new[] { NodeMode.CommentAdded }).Count(),
                cp.CompareResult.Data.FilterByMode(new[] { NodeMode.CommentRemoved }).Count(),
                cp.CompareResult.Data.FilterByMode(new[] { NodeMode.ElementTextChanged }).Count()
            };
            if (metrics.Length == 12)
            {
                metricsAfter = metricsAfter.Concat(
                    new int[]
                    {
                        cp.CompareResult.Data.FilterByMode(new[] { NodeMode.TheSameDiffInside }).Count(),
                        cp.CompareResult.Data.FilterByMode(new[] { NodeMode.FolderDiffInside }).Count()
                    }).ToArray();
            }
            Debug.WriteLine(cp.CompareResult);
            for (int i=0; i< metrics.Length; i++)
                Assert.True(metrics[i] == metricsAfter[i]);
        }

    }
}
