using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WpfApp1
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ViewModel()
        {
            ModelVisual3Ds = new ObservableCollection<ModelVisual3D>();
        }

        private ObservableCollection<ModelVisual3D> _modelVisual3Ds;
        public ObservableCollection<ModelVisual3D> ModelVisual3Ds 
        {
            get => _modelVisual3Ds;
            set
            {
                _modelVisual3Ds = value;
                NotifyPropertyChanged("ModelVisual3Ds");
            }
        }

        private ModelVisual3D _modelVisual3D;

        public ModelVisual3D ModelVisual3D
        {
            get => _modelVisual3D;
            set
            {
                _modelVisual3D = value;
                NotifyPropertyChanged("ModelVisual3D");
            }
        }

        public void Clear()
        {
            _modelVisual3Ds.Clear();
        }

    }
}
