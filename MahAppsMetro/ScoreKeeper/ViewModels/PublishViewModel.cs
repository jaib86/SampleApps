using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ScoreKeeper.ViewModels
{
    class PublishViewModel : ViewModelBase
    {
        private int progress;
        private bool isFinished;
        private DispatcherTimer timer;

        public PublishViewModel(Action closeAction)
        {
            OkCommand = new RelayCommand(_ => closeAction());
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += TimerOnTick;
            timer.IsEnabled = true;            
        }

        public RelayCommand OkCommand { get; private set; }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            Progress+=2;
            if (Progress >= 100)
            {
                IsFinished = true;
                timer.IsEnabled = false;    
            }
        }

        public int Progress
        {
            get { return progress; }
            set
            {
                if (value == this.progress) return;
                this.progress = value;
                OnPropertyChanged();
            }
        }

        public bool IsFinished
        {
            get { return isFinished; }
            set
            {
                if (value.Equals(isFinished)) return;
                isFinished = value;
                OnPropertyChanged();
            }
        }
    }
}
