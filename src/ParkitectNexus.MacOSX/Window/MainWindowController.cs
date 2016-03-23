using System;
using MonoMac.Foundation;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;

namespace ParkitectNexus.MacOSX
{
    [Register("MainWindowController")]
    public partial class MainWindowController : MonoMac.AppKit.NSWindowController
    {
        #region Constructors

        public MainWindowController(IntPtr handle) : base(handle)
        {
        }

        [Export("initWithCoder:")]
        public MainWindowController(NSCoder coder) : base(coder)
        {
        }

        public MainWindowController() : base("MainWindow")
        {
            IPresenterFactory presenterFactory = ObjectFactory.GetInstance<IPresenterFactory>();
            base.Window = presenterFactory.InstantiatePresenter<MainWindow>();
            Window.AwakeFromNib();
        }

        #endregion

        public new MainWindow Window
        {
            get
            {
                return (MainWindow)base.Window;
            }
        }
    }
}

