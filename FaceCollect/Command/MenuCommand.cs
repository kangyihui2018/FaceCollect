using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FaceCollect
{
    public class MenuCommand : Freezable, ICommand, ICommandSource
    {

        public MenuCommand()
        {
        }

        public static readonly DependencyProperty CommandParameterProperty =
          DependencyProperty.Register(
              "CommandParameter",
              typeof(object),
              typeof(MenuCommand),
              new PropertyMetadata((object)null));

        public object CommandParameter
        {
            get
            {
                return (object)GetValue(CommandParameterProperty);
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        public static readonly DependencyProperty CommandTargetProperty =
           DependencyProperty.Register(
               "CommandTarget",
               typeof(IInputElement),
               typeof(MenuCommand),
               new PropertyMetadata((IInputElement)null));

        public IInputElement CommandTarget
        {
            get
            {
                return (IInputElement)GetValue(CommandTargetProperty);
            }
            set
            {
                SetValue(CommandTargetProperty, value);
            }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), 
            typeof(MenuCommand), new PropertyMetadata(new PropertyChangedCallback(OnCommandChanged)));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (Command != null)
                return Command.CanExecute(CommandParameter);
            return false;
        }

        public void Execute(object parameter)
        {
            Command.Execute(CommandParameter);
        }

        public event EventHandler CanExecuteChanged;

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MenuCommand commandReference = d as MenuCommand;
            ICommand oldCommand = e.OldValue as ICommand;
            ICommand newCommand = e.NewValue as ICommand;

            if (oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= commandReference.CanExecuteChanged;
            }
            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += commandReference.CanExecuteChanged;
            }
        }

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
