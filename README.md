Xamarin Android Office 365 API Sample
======================================

###Introduction

This sample shows how to use query Office 365 API service from a Xamarin Android project. 

The sample builds a Xamarin binding to ADAL's native Android Library. The generated binding is then referenced by specific Android projects to authenticate and get tokens for Office 365 API resources.

ADAL team is planning on adding more robust support to Xamarin projects, but until then, Xamarin bindings will help you build the ADAL library you need for your Android projects. You can download and use [ADAL v3.0 preview](http://www.cloudidentity.com/blog/2014/10/30/adal-net-v3-preview-pcl-xamarin-support/) if you are interested to see ADAL vNext. ADAl v3.0 is a PCL and comes with support for Xamarin Android and iOS projects as well. However, please remember that ADAL v3.0 is still a preview and is not recommended to use in production. Here is an [ADAL sample that uses ADAL v3.0 preview](https://github.com/AzureADSamples/NativeClient-MultiTarget-DotNet).

This sample has threee projects:

####XamarinOffice365.AdalBindings
This is the actual Xamarin binding project. Read more here to understand how to keep up this project with the latest ADAL Android Library and build the binding project.

####XamarinOffice365.AndroidHelloworld
A simple Xamarin Hello World that authenticates and gets a token for Office 365 API discovery service. Useful if you want to have a simple project for getting started with using the ADAL Bindings for Xamarin Android.

####XamarinOffice365.Droid
This project describes the code to interact with the Exchange Contacts API and query your contacts. 

## How to Run this Sample
To run this sample, you need:

1. Visual Studio 2013
3. [Office 365 Developer Subscription](https://portal.office.com/Signup/Signup.aspx?OfferId=6881A1CB-F4EB-4db3-9F18-388898DAF510&DL=DEVELOPERPACK&ali=1)

## Step 1: Clone or download this repository
From your Git Shell or command line:

`git clone https://github.com/OfficeDev/XamarinAndroidO365.git`

## Step 2: Build the XamarinOffice365.AdalBindings Project
1. Open the project in Visual Studio 2013.
2. Go to XamarinOffice365.AdalBindings project node.
3. Build the XamarinOffice365.AdalBindings project.

## Step 4: Configure XamarinOffice365.Droid Project

### Manually Register an Azure AD Application
Follow the steps [here](http://msdn.microsoft.com/library/azure/dn132599.aspx) to manually register an application for your Android client application and copy the following from the portal:
1. Client Id
2. Redirect URI

### Update MainActivity.cs
Insert the copied Client Id and Redirect URI in the respective variables where it says:

```
internal string ClientId = "[insert-your-client-id]";
internal string RedirectUri = "[insert-your-redirect-uri]";
```
### Build and Debug the Application
Hit Debug or Press F5 to build and debug the application!

