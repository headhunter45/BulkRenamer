using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkRenamer
{
    public class SearchResultItemModel
    {
        public Boolean IsSelected { get; set; }

        private String _before;
        public String Before { get { return _before; } }

        private String _after;
        public String After { get { return _after; } }

        private String _folder;
        public String Folder { get { return _folder; } }

        public SearchResultItemModel(String folder, String before, String after)
        {
            _folder = folder;
            _before = before;
            _after = after;
            IsSelected = true;            
        }
        
    }
}
