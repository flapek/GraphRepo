using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GraphApp.ViewModel
{
    class NodeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ModelVisual3D> modelVisual3Ds { get; set; }
        public NodeViewModel()
        {
            this.modelVisual3Ds = new ObservableCollection<ModelVisual3D>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        internal void RaisePropertyChanged(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        
    }
}
