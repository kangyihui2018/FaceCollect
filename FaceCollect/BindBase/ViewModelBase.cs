using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace FaceCollect
{
    /// <summary>
    /// WINDOW、 Page 、 UserControl 对应   VIEWMODE基类
    /// </summary>
    public abstract class ViewModelBase : NotifyPropertyChanged, IViewModel
    {
        /// <summary>
        /// 显示在页面内部的错误信息
        /// </summary>
        protected string viewErrorInfo;
        public virtual string ViewErrorInfo
        {
            get { return this.viewErrorInfo; }
            set
            {
                this.viewErrorInfo = value;
                this.IsShowViewError = true;
                this.OnPropertyChanged(()=>ViewErrorInfo);
            }
        }

        /// <summary>
        /// 是否显示页面内部的错误信息显示区域
        /// </summary>
        public virtual bool IsShowViewError
        {
            get { return !string.IsNullOrEmpty(this.viewErrorInfo); }
            set
            {
                this.OnPropertyChanged(() => IsShowViewError);
            }
        }

        /// <summary>
        /// 错误信息作用UI对象
        /// </summary>
        protected FrameworkElement errorTarget;

        /// <summary>
        /// VM作用域
        /// </summary>
        protected FrameworkElement mainScope;

        /// <summary>
        /// 参数
        /// </summary>
        protected IViewArgs[] viewArgs;

        public ViewModelBase()
        {
           
        }

        /// <summary>
        /// 开启错误提示动画
        /// </summary>
        public virtual void BeginOpacityStoryboard(string error, FrameworkElement target)
        {
            if (target == null) return;

            this.ViewErrorInfo = error;
            var storyboard = Application.Current.FindResource("OpacityViewErrorTip") as Storyboard;
            if (storyboard != null)
            {
                storyboard.Begin(target);
            }
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="modelScope">ViewModel作用域</param>
        /// <param name="errorScope">错误信息显示作用域</param>
        /// <param name="args">附加参数</param>
        public virtual void OnInitialize(FrameworkElement modelScope,
            FrameworkElement errorScope = null,
            params IViewArgs[] args)
        {
            this.mainScope = modelScope;
            this.errorTarget = errorScope;
            this.viewArgs = args;
            this.mainScope.Loaded += OnBaseLoaded;
        }

        protected virtual void OnBaseLoaded(object sender, RoutedEventArgs e)
        {
           
        }
    }


    /// <summary>
    /// view model 
    /// </summary>
    public interface IViewModel
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="modelScope"></param>
        /// <param name="errorScope"></param>
        /// <param name="args"></param>
        void OnInitialize(FrameworkElement modelScope,
            FrameworkElement errorScope = null,
            params IViewArgs[] args);

        /// <summary>
        /// 错误动画
        /// </summary>
        /// <param name="error"></param>
        /// <param name="target"></param>
        void BeginOpacityStoryboard(string error, FrameworkElement target);
    }

    /// <summary>
    /// view向VIEWMODEL传参
    /// </summary>
    public interface IViewArgs
    {

    }
}
