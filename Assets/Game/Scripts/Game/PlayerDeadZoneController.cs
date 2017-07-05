using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// push the player to fall and dead
/// </summary>
public class PlayerDeadZoneController : MonoBehaviour
{
    private Player _player;

    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            int dir = other.transform.position.x > transform.position.x ? -1 : 1;

            _player.Kill(dir);
            //GameManager.instance.GameOver();
        }
            
    }
}
