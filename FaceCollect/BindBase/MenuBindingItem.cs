using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceCollect
{
    public class MenuBindingItem : DataBindingItem
    {


        protected string linkUrl;

        public string LinkUrl
        {
            get { return this.linkUrl; }
            set
            {
                this.linkUrl = value;
                this.OnPropertyChanged(() => LinkUrl);
            }
        }

        /// <summary>
        /// 是否动态菜单 
        /// </summary>
        public bool IsDynamic { get; set; }

        /// <summary>
        /// 动态菜单附加参数
        /// </summary>
        public string Args { get; set; }

        protected string text;
        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                this.OnPropertyChanged(() => Text);
            }
        }

        public override string ToString()
        {
            return this.text;
        }

        protected bool isVisible;
        public virtual bool IsVisible
        {
            get { return this.isVisible; }
            set
            {
                if (this.isVisible == value) return;
                this.isVisible = value;
                this.OnPropertyChanged(() => IsVisible);
            }
        }

        /// <summary>
        /// 互斥组
        /// </summary>
        public string MutexGroup { get; set; }

        /// <summary>
        /// 互斥控制显示隐藏
        /// true-->hide
        /// false->show
        /// </summary>
        protected bool isMutex;
        public virtual bool IsMutex
        {
            get
            {
                if (string.IsNullOrEmpty(MutexGroup)) return false;
                return this.isMutex;
            }
            set
            {
                this.isMutex = value;
                this.OnPropertyChanged(() => IsMutex);
            }
        }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public string MenuId { get; set; } = "";

        public MenuBindingItem()
        {
            this.IsVisible = true;
        }

        public MenuBindingItem(string group, string title) : this()
        {
            this.GroupName = group;
            this.text = title;
        }
    }

}
