using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using Xunit;

namespace SimpleWPFAppXUnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Opens the application
            var app = Application.Launch(@"..\..\..\SimpleWPFApp\bin\Debug\SimpleWPFApp.exe");

            // Finds the main window (this and above line should be in [TestInitialize])
            var mainWindow = app.GetWindow("Main Window", InitializeOption.NoCache);

            var button1 = mainWindow.Get<Button>("button1");

            var checkBox1 = mainWindow.Get<CheckBox>("checkBox1");

            Assert.False(checkBox1.Enabled);

            // Simulate clicking
            button1.Click();

            Task.Delay(200);

            Assert.True(checkBox1.Enabled);
        }
    }
}