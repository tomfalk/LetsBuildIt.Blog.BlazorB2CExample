using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsBuildIt.Blog.BlazorB2CExample.Client.Services
{
    public class AppState
    {
        // State
        public bool IsLoggedIn => _b2cService.IsLoggedIn;

        // Lets components receive change notifications
        // Could have whatever granularity you want (more events, hierarchy...)
        public event Action OnChange;

        // DI Services
        private B2CService _b2cService { get; set; }


        public AppState(B2CService b2cService)
        {
            _b2cService = b2cService;
            _b2cService.OnChange += LoginChange;
        }

        public Task Login()
        {
            return _b2cService.Login();
        }

        public Task Logout()
        {
            return _b2cService.Logout();
        }

        private void LoginChange()
        {
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
