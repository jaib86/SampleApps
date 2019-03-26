using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreKeeper.ViewModels
{
    class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(Action closeAction)
        {
            OkCommand = new RelayCommand(_ => closeAction());
        }
        public RelayCommand OkCommand { get; private set; }
    }
}
