﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CatsTaskProject.ViewModels.Commands
{
    public class DelegateCommand : ICommand
    {
        private Action<object> _action;
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter = null)
        {
            _action(parameter);
        }
    }
}
