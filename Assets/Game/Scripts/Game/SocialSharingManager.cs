using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialSharingManager : MonoBehaviour
{
    public Texture2D defaultImageToShare;

    private readonly string playStoreURL = "Insert URL here";
    private readonly string shareTitle = "Share me!";
    private readonly string shareContent = "Play this awesome game!";
   

    void Start()
    {
        
    }

    public void FacebookDefaultShare()
    {
        //first check if android is installed
        AndroidNativeUtility.OnPackageCheckResult += OnFacebookPackageCheckResult;
        AndroidNativeUtility.Instance.CheckIsPackageInstalled("com.facebook.katana");        
    }

    public void TwitterDefaultShare()
    {
        AndroidSocialGate.StartShareIntent(shareTitle, shareContent  + " " + playStoreURL, defaultImageToShare, "twi");
    }

    public void NativeSharing()
    {
        AndroidSocialGate.StartShareIntent(shareTitle, shareContent + " " + playStoreURL, defaultImageToShare);
    }

    public void OnFacebookPackageCheckResult(AN_PackageCheckResult res)
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
