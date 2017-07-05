using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// code for purchasing
/// </summary>
public class ShopManager : MonoBehaviour 
{
    
    //sku
    private const string PRODUCT_LIVEX5     = "livex5";
    private const string PRODUCT_LIVEX10    = "livex10";
    private const string PRODUCT_LIVEX20    = "livex20";


  
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

        int currentLives = PlayerPrefs.GetInt("lives");

        switch (purchase.Purchase.SKU)
        {
            case PRODUCT_LIVEX5:
                PlayerPrefs.SetInt("lives" , currentLives + 5);
                break;
            case PRODUCT_LIVEX10:
                PlayerPrefs.SetInt("lives", currentLives + 10);
                break;
            case PRODUCT_LIVEX20:
                PlayerPrefs.SetInt("lives", currentLives + 20);            
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
