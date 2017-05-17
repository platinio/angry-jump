using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    #region buttons
    public Button playButton;
    public Button facebookButton;
    public Button nativeShareButton;
    public Button twitterButton;
    public Button shopButton;
    public Button closeShopButton;
    #endregion buttons

    private UIScreen screen;
    
    void Start()
    {
        screen = GetComponent<UIScreen>();
        SetButtonsActions();                
    }


    private void SetButtonsActions()
    {
        playButton.onClick.AddListener(delegate { PlatinioUI.instance.MoveToNext(); });
        facebookButton.onClick.AddListener(delegate { SocialSharingManager.FacebookDefaultShare(); });
        nativeShareButton.onClick.AddListener(delegate { SocialSharingManager.NativeSharing(); });
        twitterButton.onClick.AddListener(delegate { SocialSharingManager.TwitterDefaultShare(); });
        twitterButton.onClick.AddListener(delegate { SocialSharingManager.TwitterDefaultShare(); });
        shopButton.onClick.AddListener(delegate { screen.ShowElement("ShopWindow"); });
        closeShopButton.onClick.AddListener(delegate { screen.HideElement("ShopWindow"); });
    }

}
