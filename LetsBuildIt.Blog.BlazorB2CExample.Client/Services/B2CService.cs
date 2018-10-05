using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsBuildIt.Blog.BlazorB2CExample.Client.Services
{
    public class B2CService
    {
        // Authentication State
        public bool IsLoggedIn { get; private set; }
        public string AccessToken { get; private set; }
        public string IdToken { get; private set; }

        // Login notifications
        public event Action OnChange;

        public B2CService()
        {
        }

        // Public Methods

        public Task Login()
        {
            return JSRuntime.Current.InvokeAsync<object>("B2CService.login", new DotNetObjectRef(this));
        }

        public Task Logout()
        {
            ClearSession();
            return JSRuntime.Current.InvokeAsync<object>("B2CService.logout");
        }

        // JS Invokable Methods

        [JSInvokable]
        public Task HandleLoginSuccess(string idToken, string accessToken)
        {
            return SetSession(idToken, accessToken);
        }

        [JSInvokable]
        public void HandleLoginFail()
        {
            ClearSession();
        }

        private async Task SetSession(string idToken, string accessToken)
        {
            Console.WriteLine("ID Token: " + idToken);
            Console.WriteLine("Access Token: " + accessToken);

            IdToken = idToken;
            AccessToken = accessToken;

            IsLoggedIn = true;
            NotifyLoggedIn();
        }

        private void ClearSession()
        {
            IsLoggedIn = false;
            IdToken = null;
            AccessToken = null;
            NotifyLoggedOut();
        }

        private void NotifyLoggedIn() => OnChange?.Invoke();
        private void NotifyLoggedOut() => OnChange?.Invoke();
    }
}
