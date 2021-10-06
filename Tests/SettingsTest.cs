using System;
using XmlCompare.Model;
using XmlCompare.Presenter;
using XmlCompare.View;
using Xunit;

namespace XmlCompare.Tests
{
    [Collection("Our Test Collection")]
    public class SettingsTest
    {
        [Fact]
        public void CheckNull()
        {
            var mf = new MainForm(true);
            var sp = new SettingsPresenter(mf, Settings.GetSettings());
            var cp = new ComparePresenter(mf, sp, new ReportPresenter());
            var cpn = new ComparePresenter(mf, sp, null); // ReportPresenter can be null
            Assert.NotNull(mf);
            Assert.NotNull(sp);
            Assert.NotNull(cp);
            Assert.NotNull(cpn);
            // others can not be null
            {
                bool fl = false;
                try { new ComparePresenter(null, sp, null); } catch (ArgumentNullException) { fl = true;  }
                Assert.True(fl);
            }
            {
                bool fl = false;
                try { new ComparePresenter(mf, null, null); } catch (ArgumentNullException) { fl = true; }
                Assert.True(fl);
            }
            {
                bool fl = false;
                try { new ComparePresenter(null, null, null); } catch (ArgumentNullException) { fl = true; }
                Assert.True(fl);
            }
            {
                bool fl = false;
                try { new SettingsPresenter(mf, null); } catch (ArgumentNullException) { fl = true; }
                Assert.True(fl);
            }
            {
                bool fl = false;
                try { new SettingsPresenter(null, Settings.GetSettings()); } catch (ArgumentNullException) { fl = true; }
                Assert.True(fl);
            }
            {
                bool fl = false;
                try { new SettingsPresenter(null, null); } catch (ArgumentNullException) { fl = true; }
                Assert.True(fl);
            }
        }

        [Fact]
        public void CheckSettingsModel()
        {
            // empty
            Settings SettingsModelNull = Settings.GetSettings();
            SettingsModelNull.RightFileName = null;
            SettingsModelNull.LeftFileName = null;
            SettingsModelNull.IsShowDifferences = false;
            Assert.Null(SettingsModelNull.LeftFileName);
            Assert.Null(SettingsModelNull.RightFileName);
            Assert.False(SettingsModelNull.IsShowDifferences);
            // full
            Settings SettingsModel = Settings.GetSettings();
            SettingsModel.RightFileName = "rfn";
            SettingsModel.LeftFileName = "lfn";
            SettingsModel.IsShowDifferences = true;
            Assert.Equal("lfn", SettingsModel.LeftFileName);
            Assert.Equal("rfn", SettingsModel.RightFileName);
            Assert.True(SettingsModel.IsShowDifferences);
            // from settings
            Settings SettingsModelAgain = Settings.GetSettings();
            Assert.Equal("lfn", SettingsModelAgain.LeftFileName);
            Assert.Equal("rfn", SettingsModelAgain.RightFileName);
            Assert.True(SettingsModelAgain.IsShowDifferences);
        }
    }
}
