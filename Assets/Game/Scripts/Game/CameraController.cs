using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cam controller for follow the player
 * */

public class CameraController : MonoBehaviour 
{
    
    //inspector
    [SerializeField] private float _distance = 2.0f;
    
    //private
    private Player      _player             = null;
    private Transform   _originalParent     = null; //original parent from player
    

    //UNITY_EVENTS
    void Start()
    {
        _player         = GameObject.FindObjectOfType<Player>();
        if (_player == null)
            Debug.LogError("Player Object can no be found");

        _originalParent = transform.parent; //set the original parent
    }

    void Update()
    {
        //if the player still down we return
        if (transform.position.y > _player.transform.position.y)
            return;

        //if we reach the distance follo the player making him the parent
        if (Vector2.Distance( new Vector2(0, _player.transform.position.y), new Vector2(0, transform.position.y) ) > _distance)
            transform.parent = _player.transform;
        
        else if (transform.parent != _originalParent)
            transform.parent = _originalParent;
    }
   

}
