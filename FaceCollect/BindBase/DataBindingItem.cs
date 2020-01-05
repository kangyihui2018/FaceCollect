using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceCollect
{
    /// <summary>
    /// 通用集合类型数据绑定结构基类
    /// </summary>
    public class DataBindingItem : NotifyPropertyChanged
    {
        /// <summary>
        /// 可用于选中（checkbox radiobox troggle）
        /// </summary>
        protected bool isChecked;
        public virtual bool IsChecked
        {
            get { return this.isChecked; }
            set
            {
                this.isLastChecked = this.isChecked;
                this.isChecked = value;
                this.OnPropertyChanged(() => IsChecked);
            }
        }

        private bool isLastChecked = false;
        /// <summary>
        /// 上一次的选中状态
        /// </summary>
        public bool IsLastChecked { get => isLastChecked; }

        /// <summary>
        /// 唯一ID
        /// </summary>
        public virtual string Id { get; set; }

        protected int dataType;

        /// <summary>
        /// 数据类型
        /// </summary>
        public virtual int DataType
        {
            get { return this.dataType; }
            set
            {
                this.dataType = value;
                this.OnPropertyChanged(() => DataType);
            }
        }

        /// <summary>
        /// 控件类型
        /// </summary>
        public virtual int ControlType { get; set; }
       

        /// <summary>
        /// 显示文本
        /// </summary>
        public virtual string Header { get; set; }

        public virtual int Key { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public virtual string GroupName { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
        protected bool isHide;
        public virtual bool IsHide
        {
            get { return this.isHide; }
            set
            {
                this.isHide = value;
                this.OnPropertyChanged(() => IsHide);
            }
        }


        public DataBindingItem()
        { }

        public DataBindingItem(string id, string header)
        {
            this.Id = id;
            this.Header = header;
        }

        public DataBindingItem(int key, string header)
        {
            this.Key = key;
            this.Header = header;
        }

    }
}
