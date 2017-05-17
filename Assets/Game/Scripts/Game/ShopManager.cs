using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour 
{
    public RectTransform shopWindow;

    private Vector3 startPosition;
    private bool processingAnimation;
    private bool isOpen;

    //sku
    private const string PRODUCT_LIVEX5 = "livex5";
    private const string PRODUCT_LIVEX10 = "livex10";
    private const string PRODUCT_LIVEX20 = "livex20";


  
    void Start()
    {
        //listening for Purchase and consume events
        AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;
        AndroidInAppPurchaseManager.ActionProductConsumed += OnProcessingConsumeProduct;

        //listening for store initialising finish
        AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;

        AndroidInAppPurchaseManager.Client.Connect();

        //startPosition = shopWindow.anchoredPosition3D;
    }

    public void OpenShop()
    {
        if (isOpen)
            return;

        isOpen = true;
        processingAnimation = true;
        LeanTween.moveY(shopWindow, 0, 2.0f).setEase(LeanTweenType.easeOutElastic).setOnComplete(() => { processingAnimation = false; }); ;
    }

    public void CloseShop()
    {
        if (processingAnimation)
            return;

        isOpen = false;
        LeanTween.moveY(shopWindow, -1100, 0.3f).setOnComplete(() => { shopWindow.anchoredPosition3D = startPosition; });
    }


    public void Buy(string id)
    {
        AndroidInAppPurchaseManager.Client.Purchase(id);
    }

    //purchasing events
    private void OnProductPurchased(BillingResult result)
    {
        if (result.IsSuccess)
            OnProcessingPurchasedProduct(result.Purchase);
        
        else
        {
            AndroidMessage.Create("Product Purchase Failed", result.Response.ToString() + " " + result.Message);
        }
        
    }

    private void OnProcessingPurchasedProduct(GooglePurchaseTemplate purchase)
    {
        
        switch (purchase.SKU)
        {
            case PRODUCT_LIVEX5:
                AndroidInAppPurchaseManager.Client.Consume(PRODUCT_LIVEX5);
                break;
            case PRODUCT_LIVEX10:
                AndroidInAppPurchaseManager.Client.Consume(PRODUCT_LIVEX10);
                break;
            case PRODUCT_LIVEX20:
                AndroidInAppPurchaseManager.Client.Consume(PRODUCT_LIVEX10);
                break;
        }                

    }

    private static void OnProcessingConsumeProduct(BillingResult purchase)
    {
        switch (purchase.Purchase.SKU)
        {
            case PRODUCT_LIVEX5:
                //add lives
                break;
            case PRODUCT_LIVEX10:
                //add lives
                break;
            case PRODUCT_LIVEX20:
                //add lives                
                break;
        }
    }

    private void OnBillingConnected(BillingResult result)
    {
        AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;

        if (!result.IsSuccess)
        {
            AndroidMessage msg = AndroidMessage.Create("Error!", "Error trying to connect to billing service");
        }
        else
        {
            AndroidMessage msg = AndroidMessage.Create("Connected", "Connected to billing service");
        }
 
    }

    
}
