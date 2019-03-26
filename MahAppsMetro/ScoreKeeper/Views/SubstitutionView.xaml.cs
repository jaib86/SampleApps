using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScoreKeeper.Views
{
    /// <summary>
    /// Interaction logic for SubstitutionView.xaml
    /// </summary>
    public partial class SubstitutionView : UserControl
    {
        public SubstitutionView()
        {
            InitializeComponent();
        }

        private void OnButtonOkClick(object sender, RoutedEventArgs e)
        {
            var w = Window.GetWindow(this);
            w.DialogResult = true;
            w.Close();
        }
    }
}
