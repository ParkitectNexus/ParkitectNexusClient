// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

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

        public AuthenticateTask(IAuthManager authManager)
        {
            if (authManager == null) throw new ArgumentNullException(nameof(authManager));
            _authManager = authManager;

            Name = "Log in";
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

            UpdateStatus($"You are now logged in as {user.Name}!", 100, TaskStatus.Stopped);
        }

        #endregion
    }
}
