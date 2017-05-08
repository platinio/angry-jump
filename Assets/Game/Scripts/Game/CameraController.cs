using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cam controller for follow the player
 * */

public class CameraController : MonoBehaviour 
{
    

    public float minDistanceFromPlayer;

    #region PRIVATE_FIELDS
    private Player player;
    private Transform originalParent; //original parent from player
    #endregion PRIVATE_FIELDS

    #region UNITY_EVENTS
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        originalParent = transform.parent; //set the original parent
    }

    void Update()
    {
        //if the player still down we return
        if (transform.position.y > player.transform.position.y)
            return;

        if (Vector2.Distance(new Vector2(0, player.transform.position.y), new Vector2(0, transform.position.y)) > minDistanceFromPlayer)
            transform.parent = player.transform;
        
        else if (transform.parent != originalParent)
            transform.parent = originalParent;
    }
    #endregion UNITY_EVENTS

}
