using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CatsTaskProject.Models
{
    public class ComboItem : ReactiveObject
    {
        private bool _ischecked;
        public bool Ischecked
        {
            get => _ischecked;
            set => this.RaiseAndSetIfChanged(ref _ischecked, value);
        }

        private string _value;
        public string Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }

        public ComboItem(string val)
        {
            Value = val;
            Ischecked = false;
        }

    }
}
