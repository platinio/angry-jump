using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadZoneController : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            int dir = other.transform.position.x > transform.position.x ? -1 : 1;

            player.Kill(dir);
            //GameManager.instance.GameOver();
        }
            
    }
}
