using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopWindow : MonoBehaviour
{
    public Button buyFiveLives;
    public Button buyTenLives;
    public Button buyTwentyLives;

    private ShopManager shopManager;

    void Start()
    {
        shopManager = GameObject.FindObjectOfType<ShopManager>();

        SetButtonReferences();
    }

    private void SetButtonReferences()
    {
        buyFiveLives.onClick.AddListener(delegate { shopManager.Buy("livex5"); });
        buyTenLives.onClick.AddListener(delegate { shopManager.Buy("livex10"); });
        buyTwentyLives.onClick.AddListener(delegate { shopManager.Buy("livex20"); });
    }
}
