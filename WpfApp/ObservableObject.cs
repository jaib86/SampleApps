using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    [Serializable()]
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SetAndNotify<T>(ref T field, T value, Expression<Func<T>> property)
        {
            if (!object.ReferenceEquals(field, value))
            {
                field = value;
                this.OnPropertyChanged(property);
            }
        }

        protected virtual void Notify<T>(ref T field, T value, Expression<Func<T>> property)
        {
            this.OnPropertyChanged(property);
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> changedProperty)
        {
            if (PropertyChanged != null)
            {
                string name = ((MemberExpression)changedProperty.Body).Member.Name;
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #region Common Fields & Properties

        private int viewFontSize = 10;

        public int ViewFontSize
        {
            get
            {
                return this.viewFontSize;
            }
            set
            {
                this.SetAndNotify(ref this.viewFontSize, value, () => this.ViewFontSize);
            }
        }

        #endregion Common Fields & Properties
    }
}
