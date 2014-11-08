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
using System;

namespace XamarinOffice365.Android
{
    [Activity(Label = "XamarinOffice365.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
       internal string ClientId = "[insert-your-client-id]";
        internal string RedirectUri = "[insert-your-redirect-uri]";
        internal string Authority = "https://login.windows.net/Common";
        internal readonly Uri discoveryServiceEndpointUri = new Uri("https://api.office.com/discovery/v1.0/me/");
        internal readonly string discoveryServiceResourceId = "https://api.office.com/discovery/";

        int count = 1;

        AuthenticationContext authContext;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            DefaultTokenCacheStore cache = new DefaultTokenCacheStore(this);
            authContext = new AuthenticationContext(this, "https://login.windows.net/Common/", false, cache);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate 
            { 
                button.Text = string.Format("{0} clicks!", count++);

                authContext.AcquireToken(this, discoveryServiceResourceId, ClientId, RedirectUri, PromptBehavior.Auto, new TestCallback(this));
            };
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (authContext != null)
            {
                authContext.OnActivityResult(requestCode, (int)resultCode, data);
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
                builder.Create().Show();
            }

            public void OnSuccess(Java.Lang.Object result)
            {                
                AuthenticationResult aresult = result.JavaCast<AuthenticationResult>();
                if (aresult != null)
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(context);
                    builder.SetMessage(aresult.AccessToken);
                    builder.SetTitle(aresult.ExpiresOn.ToString());
                    builder.Create().Show();
                }
            }
        }
        
    }
}

