using System.ComponentModel;

namespace ComputerAccounting
{
    public enum NameView
    {
        [Description("Элемент")]
        Control,
        [Description("Страница")]
        Page,
        [Description("Менеджер")]
        Manager
    }

    public class ViewEventArgs
    {
        public IViewSwitcher ViewToLoad { get; set; }
        public NameView NameView { get; set; }

        public ViewEventArgs(IViewSwitcher viewToLoad, NameView nameView)
        {
            ViewToLoad = viewToLoad;
            NameView = nameView;
        }
    }

    public delegate void ViewHandler(object sender, ViewEventArgs e);

    public interface IViewSwitcher
    {
        event ViewHandler SwitchView;
    }
}