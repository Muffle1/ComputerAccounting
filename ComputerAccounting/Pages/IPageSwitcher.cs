using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ComputerAccounting.Pages
{
    public class PageEventArgs
    {
        internal IPageSwitcher PageToLoad { get; set; }

        internal PageEventArgs(IPageSwitcher pageToLoad)
        {
            PageToLoad = pageToLoad;
        }
    }

    public delegate void PageHandler(object sender, PageEventArgs e);

    public interface IPageSwitcher
    {
        event PageHandler SwitchPage;
    }
}
