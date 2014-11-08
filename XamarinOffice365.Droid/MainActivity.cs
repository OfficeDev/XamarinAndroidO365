//----------------------------------------------------------------------------------------------
//    Copyright 2014 Microsoft Corporation
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//----------------------------------------------------------------------------------------------

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Com.Microsoft.Aad.Adal;
using Microsoft.Office365.Discovery;
using Microsoft.Office365.OutlookServices;
using System;
using System.Threading.Tasks;


namespace XamarinOffice365.Droid
{
    [Activity(Label = "XamarinOffice365.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        AuthenticationContext authContext;
        DefaultTokenCacheStore LocalAccountCache;
        internal UserInfo AadUserInfo;

        internal string ClientId = "[insert-your-client-id]";
        internal string RedirectUri = "[insert-your-redirect-uri]";
        internal string Authority = "https://login.windows.net/Common";
        internal readonly Uri discoveryServiceEndpointUri = new Uri("https://api.office.com/discovery/v1.0/me/");
        internal readonly string discoveryServiceResourceId = "https://api.office.com/discovery/";        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            DefaultTokenCacheStore authTokenCache = new DefaultTokenCacheStore(this);
            authContext = new AuthenticationContext(this, Authority, false, authTokenCache);

            button.Click += button_Click;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (authContext != null)
            {
                authContext.OnActivityResult(requestCode, (int)resultCode, data);
            }
        }

        async void GetContacts()
        {
            DiscoveryClient discoveryClient = new DiscoveryClient(discoveryServiceEndpointUri,
                () =>
                {
                    var authResult = authContext.AcquireTokenSilentSync(discoveryServiceResourceId, ClientId, AadUserInfo.UserId);

                    return authResult.AccessToken;
                });

            var dcr = await discoveryClient.DiscoverCapabilityAsync("Contacts");

            OutlookServicesClient outlookContactsClient = new OutlookServicesClient(dcr.ServiceEndpointUri,
                async () =>
                {
                    var authResult = authContext.AcquireTokenSilentSync(dcr.ServiceResourceId, ClientId, AadUserInfo.UserId);

                    return await Task.FromResult<string>(authResult.AccessToken);
                });

            var c = await outlookContactsClient.Me.Contacts.ExecuteAsync();
            var cs = c.CurrentPage;
            foreach (var ci in cs)
            {
                
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetMessage(ci.DisplayName);
                builder.SetTitle("Contact");
                builder.SetCancelable(false);
                builder.SetPositiveButton("OK", (sender, args) => { });
                builder.Create().Show();
            }

            if(AadUserInfo == null)
            {
                AadUserInfo = new UserInfo();
            }
        }
        void button_Click(object sender, EventArgs e)
        {
            AuthenticationResult authenticationResult = null;
            try
            {               
                authenticationResult = authContext.AcquireTokenSilentSync(discoveryServiceResourceId, ClientId, AadUserInfo.UserId);

            }
            catch (Exception exception)
            {
                //needs prompt
            }

            if (authenticationResult == null || authenticationResult.Status != AuthenticationResult.AuthenticationStatus.Succeeded)
            {
                authContext.AcquireToken(this, discoveryServiceResourceId, ClientId, RedirectUri, PromptBehavior.Auto, new TestCallback(this));
            }
            else
            {
                GetContacts();
            }
        }

        class TestCallback : Java.Lang.Object, IAuthenticationCallback
        {
            Context context;

            public TestCallback(Context ctx)
            {
                context = ctx;
            }

            public void OnError(Java.Lang.Exception exc)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(context);
                builder.SetTitle("Error");
                builder.SetMessage(exc.Message);
                builder.SetCancelable(false);
                builder.SetPositiveButton("OK", (sender, args) => { });
                builder.Create().Show();
            }

            public void OnSuccess(Java.Lang.Object result)
            {
                AuthenticationResult aresult = result.JavaCast<AuthenticationResult>();
                if (aresult != null)
                {
                    ((MainActivity)context).AadUserInfo = aresult.UserInfo;

                    ((MainActivity)context).GetContacts();
                }
            }
        }
    }
}

