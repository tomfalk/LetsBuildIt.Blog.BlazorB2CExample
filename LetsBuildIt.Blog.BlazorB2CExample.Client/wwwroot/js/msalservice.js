var B2CService = function () {
    var applicationConfig = {
        clientID: 'c17514de-1ccc-4bca-b4c7-92d1f8eb506a',
        authority: 'https://login.microsoftonline.com/tfp/letsbuildit.onmicrosoft.com/B2C_1_SiUpIn',
        scopes: ['https://letsbuildit.onmicrosoft.com/demoapi/demo.read', 'https://letsbuildit.onmicrosoft.com/demoapi/demo.write']
    };

    
    var userAgentApplication = new Msal.UserAgentApplication(applicationConfig.clientID, applicationConfig.authority, handleAuth, { cacheLocation: 'localStorage' });

    function handleAuth(errorDesc, token, error, tokenType) {
        console.log('Handle auth called.  This function does not seem to get called but is included in the MSAL documentation.');
        if (token) {
            console.log("Success");
        }
        else {
            console.log(error + ":" + errorDesc);
        }
    }

    return {
        login: function (dotnetHelper) {
            userAgentApplication.loginPopup(applicationConfig.scopes).then(function (idToken) {
                //Login Success
                userAgentApplication.acquireTokenSilent(applicationConfig.scopes).then(function (accessToken) {
                    //AcquireTokenSilent Success
                    dotnetHelper.invokeMethodAsync('HandleLoginSuccess', idToken, accessToken);
                }, function (error) {
                    //AcquireTokenSilent Failure, send an interactive request.
                    userAgentApplication.acquireTokenPopup(applicationConfig.scopes).then(function (accessToken) {
                        dotnetHelper.invokeMethodAsync('HandleLoginSuccess', idToken, accessToken);
                    }, function (error) {
                        console.log(error);
                    });
                })
            }, function (error) {
                console.log(error);
            });
        },
        logout: function () {
            userAgentApplication.logout();
        }
    };
}();