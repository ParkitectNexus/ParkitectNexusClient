// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Presenter;
using Xwt;

namespace ParkitectNexus.Client.Base.Main
{
    public class MainWindow : Window, IPresenter
    {
        public MainWindow(IPresenterFactory presenterFactory)
        {
            Title = "ParkitectNexus Client";
            Width = 950;
            Height = 650;
            Icon = App.Images["parkitectnexus_logo-64x64.png"];


            Content = presenterFactory.InstantiatePresenter<MainView>();
        }

        #region Overrides of WindowFrame

        /// <summary>
        ///     Called to check if the window can be closed
        /// </summary>
        /// <returns><c>true</c> if the window can be closed, <c>false</c> otherwise</returns>
        protected override bool OnCloseRequested()
        {
            if (base.OnCloseRequested())
            {
                Application.Exit();
                return true;
            }
            return false;
        }

        #endregion
    }
}