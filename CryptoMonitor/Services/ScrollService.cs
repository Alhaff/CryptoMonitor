using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CryptoMonitor.Services
{
    public class ScrollService
    {
        public ScrollViewer ScrollViewer { get; set; }

        public ScrollService()
        {
            ScrollViewer = new ScrollViewer();
            ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            ScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
        }
    }
}
