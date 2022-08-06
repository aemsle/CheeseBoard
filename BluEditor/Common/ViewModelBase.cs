using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BluEditor
{
    [DataContract(IsReference = true)]
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string in_property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(in_property));
        }
    }
}