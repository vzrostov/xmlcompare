using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlCompare.Presenter;
using XmlCompare.View;
using Xunit;

namespace XmlCompare.Tests
{
    public class CompareTest
    {
        [Theory]
        [InlineData(@"../../Tests/xml/test.xml", @"../../Tests/xml/test.xml")]
        [InlineData(@"../../Tests/xml/empty.xml", @"../../Tests/xml/empty.xml")]
        void CompareForEqual(string f1, string f2)
        {
            var mf = new MainForm(true);
            var sp = new SettingsPresenter(mf);
            var cp = new ComparePresenter(mf, sp, null);
            //var file = new File(f1);
            File.Create(f1);
        }

        [Theory]
        [InlineData(@"../../Tests/xml/test.xml", @"../../Tests/xml/testCorrupted.xml")]
        [InlineData(@"../../Tests/xml/testCorrupted.xml", @"../../Tests/xml/test.xml")]
        [InlineData("absent.xml", @"../../Tests/xml/test.xml")]
        [InlineData(@"../../Tests/xml/test.xml", "absent.xml")]
        void CompareForCorrupted(string f1, string f2)
        {
            var mf = new MainForm();
            var sp = new SettingsPresenter(mf);
            var cp = new ComparePresenter(mf, sp, null);
        }

    }
}
