using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopWindow : MonoBehaviour
{
    [SerializeField] private Button _buyFiveLives       = null;
    [SerializeField] private Button _buyTenLives        = null;
    [SerializeField] private Button _buyTwentyLives     = null;

    private ShopManager _shopManager    = null;

    void Start()
    {
        _shopManager = GameObject.FindObjectOfType<ShopManager>();
        SetButtonReferences();
    }

    private void SetButtonReferences()
    {
        _buyFiveLives.onClick.AddListener       (delegate { _shopManager.Buy("livex5");  });
        _buyTenLives.onClick.AddListener        (delegate { _shopManager.Buy("livex10"); });
        _buyTwentyLives.onClick.AddListener     (delegate { _shopManager.Buy("livex20"); });
    }
}
