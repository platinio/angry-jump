using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlls a coin
/// </summary>
public class CoinController : MonoBehaviour 
{
    [SerializeField] private bool _followPlayer = false;

    private Transform _player = null;

    void Start()
    {
        if(_followPlayer)
            _player = GameObject.FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        if (!_followPlayer)
            return;

        transform.position = new Vector2(_player.transform.position.x , transform.position.y);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //if we collision the player
        if (other.gameObject.CompareTag(Constants.instance.tags.player))
        {
            GameManager.instance.AlingPlatforms();
            GameManager.instance.CreateCoin();
            Destroy(gameObject);            
        }
           
    }
}
