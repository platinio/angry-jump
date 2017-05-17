using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* code for social sharing*/

public static class SocialSharingManager 
{
    private static Texture2D defaultImageToShare
    {
        get 
        { 

            if(_defaultImageToShare == null)
                _defaultImageToShare = Resources.Load("SocialShare/MainMenu") as Texture2D;
            return _defaultImageToShare;
        }
        set { _defaultImageToShare = value; }
    }

    private static Texture2D _defaultImageToShare;

    private static readonly string playStoreURL = "Insert URL here";
    private static readonly string shareTitle = "Share me!";
    private static readonly string shareContent = "Play this awesome game!";
   

    
    public static void FacebookDefaultShare()
    {
        
        //first check if android is installed
        AndroidNativeUtility.OnPackageCheckResult += OnFacebookPackageCheckResult;
        AndroidNativeUtility.Instance.CheckIsPackageInstalled("com.facebook.katana");        
    }

    public static void TwitterDefaultShare()
    {
        AndroidSocialGate.StartShareIntent(shareTitle, shareContent  + " " + playStoreURL, defaultImageToShare, "twi");
    }

    public static void NativeSharing()
    {
        AndroidSocialGate.StartShareIntent(shareTitle, shareContent + " " + playStoreURL, defaultImageToShare);
    }

    private static void OnFacebookPackageCheckResult(AN_PackageCheckResult res)
    {
        if(res.IsSucceeded)
        {
            //if is installed lest shared it
            AndroidSocialGate.StartShareIntent(shareTitle, shareContent + " " + playStoreURL, defaultImageToShare, "facebook.katana");
        }

        else
        {
            AndroidMessage msg = AndroidMessage.Create("Share error!", "Please before install facebook app");
        }

        AndroidNativeUtility.OnPackageCheckResult -= OnFacebookPackageCheckResult;

    }

}
