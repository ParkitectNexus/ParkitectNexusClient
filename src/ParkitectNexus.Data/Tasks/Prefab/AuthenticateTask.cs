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

using System;
using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.Data.Authentication;
using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Data.Tasks.Prefab
{
    public class AuthenticateTask : UrlQueueableTask
    {
        private readonly IAuthManager _authManager;

        public AuthenticateTask(IAuthManager authManager) : base("Log in")
        {
            if (authManager == null) throw new ArgumentNullException(nameof(authManager));
            _authManager = authManager;

            StatusDescription = "Try to log in.";
        }

        #region Overrides of QueueableTask

        public override async Task Run(CancellationToken token)
        {
            var data = Data as AuthUrlAction;
            if (data == null)
            {
                UpdateStatus("Invalid URL supplied.", 100, TaskStatus.Canceled);
                return;
            }

            UpdateStatus("Getting user information...", 25, TaskStatus.Running);

            if (_authManager.IsAuthenticated)
                _authManager.Logout();

            _authManager.Key = data.Key;

            var user = await _authManager.GetUser();
            UpdateStatus("Getting user avatar...", 75, TaskStatus.Running);
            await _authManager.GetAvatar();

            UpdateStatus($"You are now logged in as {user.Name}!", 100, TaskStatus.Finished);
        }

        #endregion
    }
}