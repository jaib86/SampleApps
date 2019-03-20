using System;
using System.Windows;
using System.Windows.Controls;

namespace SimpleWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void ProgressBarDelegate(DependencyProperty dp, object value);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            double progress = 0;

            ProgressBarDelegate updatePbDelegate = new ProgressBarDelegate(progressBar1.SetValue);

            do
            {
                progress++;

                Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { ProgressBar.ValueProperty, progress });

                progressBar1.Value = progress;
            }
            while (progressBar1.Value != progressBar1.Maximum);

            checkBox1.IsEnabled = true;
        }
    }
}