using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SamuraiApp.Domain
{
    public class ClientChangeTracker : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isDirty;

        public bool IsDirty
        {
            get => this.isDirty;
            set => this.SetWithNotify(value, ref this.isDirty);
        }

        protected void SetWithNotify<T>(T value, ref T field, [CallerMemberName] string propertyName = "")
        {
            if (!Equals(field, value))
            {
                field = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}