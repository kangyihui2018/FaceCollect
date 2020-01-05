using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceCollect
{
    /// <summary>
    /// 向客户端发出属性值已更改的通知--可选择项
    /// </summary>
    public abstract class SelectedItemBase : NotifyPropertyChanged
    {
        public virtual SelectedItemInfo SelectedItem { get; set; }

        public override void OnIsSelectedChanged()
        {
            if (this.SelectedItem == null) return;

            if (this.IsSelected)
            {
                this.SelectedItem.SelectedItem = this;
            }
        }
    }
}
