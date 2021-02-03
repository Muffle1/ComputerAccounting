using System.ComponentModel;

namespace ComputerAccounting.Pages
{
    public enum NameView
    {
        [Description("Страница")]
        Page = 1,
        [Description("Менеджер")]
        Manager
    }

    public class PageEventArgs
    {
        public IPageSwitcher PageToLoad { get; set; }
        public NameView NameView { get; set; }

        public PageEventArgs(IPageSwitcher pageToLoad, NameView nameView)
        {
            PageToLoad = pageToLoad;
            NameView = nameView;
        }
    }

    public delegate void PageHandler(object sender, PageEventArgs e);

    public interface IPageSwitcher
    {
        event PageHandler SwitchPage;
        
    }
}
