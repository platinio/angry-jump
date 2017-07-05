using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* code for social sharing*/

public static class SocialSharingManager 
{
    //the default image to share
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

    private static Texture2D        _defaultImageToShare    = null;
    private static readonly string  _playStoreURL           = "Insert URL here";
    private static readonly string  _shareTitle             = "Share me!";
    private static readonly string  _shareContent           = "Play this awesome game!";
   

    
    public static void FacebookDefaultShare()
    {        
        //first check if facebook is installed
        AndroidNativeUtility.OnPackageCheckResult += OnFacebookPackageCheckResult;
        AndroidNativeUtility.Instance.CheckIsPackageInstalled("com.facebook.katana");        
    }

    public static void TwitterDefaultShare()
    {
        AndroidSocialGate.StartShareIntent(_shareTitle, _shareContent  + " " + _playStoreURL, defaultImageToShare, "twi");
    }

    public static void NativeSharing()
    {
        AndroidSocialGate.StartShareIntent(_shareTitle, _shareContent + " " + _playStoreURL, defaultImageToShare);
    }

    private static void OnFacebookPackageCheckResult(AN_PackageCheckResult res)
    {
        if(res.IsSucceeded)
        {
            //if is installed lest shared it
            AndroidSocialGate.StartShareIntent(_shareTitle, _shareContent + " " + _playStoreURL, defaultImageToShare, "facebook.katana");
        }

        else
        {
            AndroidMessage msg = AndroidMessage.Create("Share error!", "Please before install facebook app");
        }

        AndroidNativeUtility.OnPackageCheckResult -= OnFacebookPackageCheckResult;

    }

}
