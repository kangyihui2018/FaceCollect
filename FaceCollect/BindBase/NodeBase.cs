using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace FaceCollect
{
    /// <summary>
    /// 树节点基类
    /// </summary>
    public class NodeBase : SelectedItemBase
    {
        /// <summary>
        /// 孩子节点
        /// </summary>
        public ObservableCollection<NodeBase> Childs { get; set; }

        /// <summary>
        /// 名字发生变化
        /// </summary>
        public Action<NodeBase> NameChanged = delegate { };

        /// <summary>
        /// ui展示类型
        /// </summary>
        public virtual int ShowType { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public virtual int DataType { get; set; }

        protected string imageSource;

        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                this.OnPropertyChanged(() => ImageSource);
            }
        }

        /// <summary>
        /// 节点ID
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// 父节点编号
        /// </summary>
        public virtual string ParentId { get; set; }


        protected string name;
        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    this.OnNameChanged();
                }

                this.OnPropertyChanged(() => Name);
            }
        }

        protected string text;

        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                this.OnPropertyChanged(() => Text);
            }
        }

        /// <summary>
        /// 排序索引
        /// </summary>
        protected int sortIndex;
        public int SortIndex
        {
            get { return sortIndex; }
            set
            {
                if (sortIndex != value)
                {
                    sortIndex = value;
                    this.OnSortIndexChanged();
                }
            }
        }

        /// <summary>
        /// 孩子数量
        /// </summary>
        public int Count
        {
            get { return this.Childs.Count; }
        }


        protected bool isExpanded;

        public virtual bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                this.RaisePropertyChanged("IsExpanded");
            }
        }


        public Object Tag { get; set; }

        public NodeBase()
        {
            this.Childs = new ObservableCollection<NodeBase>();
        }

        /// <summary>
        /// 节点名称修改；
        /// </summary>
        protected virtual void OnNameChanged()
        {
            this.NameChanged(this);
        }

        /// <summary>
        /// 排序发生变化 
        /// </summary>
        protected virtual void OnSortIndexChanged() { }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="node"></param>
        public virtual void Add(NodeBase node)
        {
            this.Childs.Add(node);
        }

        /// <summary>
        /// 移除
        /// </summary>
        public virtual void Remove(NodeBase node)
        {
            this.Childs.Remove(node);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="index"></param>
        public virtual void Insert(int index = 0)
        {
            this.Childs.Insert(index, this);
        }

        /// <summary>
        /// 索引
        /// </summary>
        /// <returns></returns>
        public virtual int Index()
        {
            return this.Childs.IndexOf(this);
        }

        /// <summary>
        /// 节点选中修改
        /// </summary>
        public override void OnIsSelectedChanged()
        {
            if (this.SelectedItem == null) return;

            if (this.IsSelected)
            {
                this.SelectedItem.SelectedItem = this;
            }
        }

        public virtual void OnManualSelectNode()
        {
            this.isSelected = true;
            this.OnPropertyChanged(() => IsSelected);
        }
    }
}
