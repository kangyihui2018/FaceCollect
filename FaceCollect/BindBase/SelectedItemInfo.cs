using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceCollect
{
    /// <summary>
    /// 选中项
    /// </summary>
    public class SelectedItemInfo : NotifyPropertyChanged
    {
        protected SelectedItemBase selectedItem;

        public SelectedItemBase SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (this.selectedItem != value)
                {
                    selectedItem = value;
                    if (this.selectedItem == null) return;
                    OnSelectedItemChanged();
                }
            }
        }

        public event EventHandler SelectedItemChanged = delegate { };

        protected virtual void OnSelectedItemChanged()
        {
            this.SelectedItemChanged(this, EventArgs.Empty);
        }
    }
}
