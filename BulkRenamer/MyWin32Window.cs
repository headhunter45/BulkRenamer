using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkRenamer
{
    public class MyWin32Window : System.Windows.Forms.IWin32Window
    {
        private readonly System.IntPtr _handle;

        public MyWin32Window(System.IntPtr handle)
        {
            _handle = handle;
        }

        System.IntPtr System.Windows.Forms.IWin32Window.Handle
        {
            get { return _handle; }
        }
    }
}
