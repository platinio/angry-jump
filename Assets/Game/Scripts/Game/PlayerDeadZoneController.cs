using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadZoneController : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
        GameManager.instance.GameOver();
    }
}
