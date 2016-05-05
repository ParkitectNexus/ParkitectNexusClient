// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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