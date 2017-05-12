using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour 
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Constants.instance.tags.player))
        {
            GameManager.instance.AlingPlatforms();
            GameManager.instance.CreateCoin();
            Destroy(gameObject);
            
        }
           
    }
}
