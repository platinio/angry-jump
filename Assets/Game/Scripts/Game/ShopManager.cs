using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour 
{
    public RectTransform shopWindow;

    private Vector3 startPosition;
    private bool processingAnimation;

    void Start()
    {
        startPosition = shopWindow.anchoredPosition3D;
    }

    public void OpenShop()
    {
        processingAnimation = true;
        LeanTween.moveY(shopWindow, 0, 2.0f).setEase(LeanTweenType.easeOutElastic).setOnComplete(() => { processingAnimation = false; }); ;
    }

    public void CloseShop()
    {
        if (processingAnimation)
            return;

        LeanTween.moveY(shopWindow, -1100, 0.3f).setOnComplete(() => { shopWindow.anchoredPosition3D = startPosition; });
    }

}
