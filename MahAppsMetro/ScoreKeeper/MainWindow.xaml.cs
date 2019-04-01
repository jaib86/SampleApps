using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using ScoreKeeper.ViewModels;

namespace ScoreKeeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.DataContext = new MainWindowViewModel();
        }


        
    }
}
