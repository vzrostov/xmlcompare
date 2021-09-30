using XmlCompare.Model;
using XmlCompare.Presenter;
using XmlCompare.View;
using Xunit;

namespace XmlCompare.Tests
{
    public class SettingsTest
    {
        [Fact]
        public void CheckNull()
        {
            var mf = new MainForm(true);
            var sp = new SettingsPresenter(mf);
            var cp = new ComparePresenter(mf, sp);
            Assert.NotNull(mf);
            Assert.NotNull(sp);
            Assert.NotNull(cp);
        }

        [Fact]
        public void CheckSettingsModel()
        {
            // empty
            Settings SettingsModelNull = new Settings();
            Assert.Null(SettingsModelNull.LeftFileName);
            Assert.Null(SettingsModelNull.RightFileName);
            Assert.False(SettingsModelNull.IsShowDifferences);
            // partly
            Settings SettingsModel = new Settings();
            SettingsModel.LeftFileName = "lfn";
            Assert.Equal("lfn", SettingsModel.LeftFileName);
            Assert.Null(SettingsModel.RightFileName);
            Assert.False(SettingsModel.IsShowDifferences);
            // fully
            SettingsModel.RightFileName = "rfn";
            SettingsModel.IsShowDifferences = true;
            Assert.Equal("lfn", SettingsModel.LeftFileName);
            Assert.Equal("rfn", SettingsModel.RightFileName);
            Assert.True(SettingsModel.IsShowDifferences);
            // from settings
            //SettingsModel.RightFileName = "rfn";
            //SettingsModel.IsShowDifferences = true;
            //SettingsPresenter sp = new SettingsPresenter(null);
            //sp.GetSettings();
        }

    }
}
