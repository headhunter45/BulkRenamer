using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkRenamer
{
    public class SearchResultItemModel: INotifyPropertyChanged
    {
        private Boolean _isSelected;
        public Boolean IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                this.NotifyPropertyChanged("IsSelected");
            }
        }

        private String _before;
        public String Before { get { return _before; } }

        private String _after;
        public String After { get { return _after; } }

        private String _folder;

        public event PropertyChangedEventHandler PropertyChanged;

        public String Folder { get { return _folder; } }

        public SearchResultItemModel(String folder, String before, String after)
        {
            _folder = folder;
            _before = before;
            _after = after;
            IsSelected = true;            
        }
        
        private void NotifyPropertyChanged(String propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
