using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;

namespace SimpleWPFAppUITests
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    // Opens the application
        //    var app = Application.Launch(@"..\..\..\SimpleWPFApp\bin\Debug\SimpleWPFApp.exe");

        //    // Finds the main window (this and above line should be in [TestInitialize])
        //    var mainWindow = app.GetWindow("Main Window", InitializeOption.NoCache);

        //    var button1 = mainWindow.Get<Button>("button1");

        //    var checkBox1 = mainWindow.Get<CheckBox>("checkBox1");

        //    Assert.AreEqual(false, checkBox1.Enabled);

        //    // Simulate clicking
        //    button1.Click();

        //    Task.Delay(200);

        //    Assert.AreEqual(true, checkBox1.Enabled);
        //}
    }
}