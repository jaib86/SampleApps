using System.IO;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using Xunit;

namespace SimpleWPFAppTests
{
    public class UITests
    {
        [Fact]
        public async Task HitButton()
        {
            // Opens the application
            var app = Application.Launch(@"D:\practices\code\SampleApps\CodedUITest\SimpleWPFApp\bin\Debug\SimpleWPFApp.exe");

            // Finds the main window (this and above line should be in [TestInitialize])
            var mainWindow = app.GetWindow("Main Window", InitializeOption.NoCache);

            var button1 = mainWindow.Get<Button>("button1");

            var checkBox1 = mainWindow.Get<CheckBox>("checkBox1");

            Assert.False(checkBox1.Enabled);

            // Simulate clicking
            button1.Click();

            var progressBar1 = mainWindow.Get<ProgressBar>("progressBar1");

            do
            {
                using (StreamWriter sw = new StreamWriter("a.txt"))
                {
                    sw.WriteLine(progressBar1.Value);
                }

                await Task.Delay(100);
            } while (progressBar1.Value != 10000);

            Assert.True(checkBox1.Enabled);
        }
    }
}