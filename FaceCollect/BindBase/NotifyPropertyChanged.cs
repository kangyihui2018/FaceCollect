using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FaceCollect
{
    /// <summary>
    /// 所有INotifyPropertyChanged接口的基类
    /// </summary>
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(((MemberExpression)expression.Body).Member.Name);
                PropertyChanged(this, e);
            }
        }

        public virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                propertyChanged(this, e);
            }
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            this.RaisePropertyChanged(propertyName);
        }

        /// <summary>
        /// 选中--列表/Checkbox
        /// </summary>
        protected bool isSelected;

        public virtual bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    this.OnIsSelectedChanged();
                    this.RaisePropertyChanged("IsSelected");
                }
            }
        }

        public virtual void OnIsSelectedChanged()
        {

        }
    }
}
