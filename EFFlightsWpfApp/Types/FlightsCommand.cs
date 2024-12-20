﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EFFlightsWpfApp.Types
{
    public class FlightsCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool>? canExecute;

        public event EventHandler? CanExecuteChanged;

        public FlightsCommand(Action<object> execute, Func<object, bool>? canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
           return canExecute is null || canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            execute?.Invoke(parameter);
        }
    }
}
