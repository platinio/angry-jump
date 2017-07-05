using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cam controller for follow the player
 * */

public class CameraController : MonoBehaviour 
{
    
    //inspector
    [SerializeField] private float _distance            = 2.0f;
    [SerializeField] private float _smooth              = 1.5f;
    [SerializeField] private float _stoppingDistance    = 0.5f;
    
    //private
    private Player      _player             = null;
    private Transform   _originalParent     = null; //original parent from player
    private float _cameraZ;

    //UNITY_EVENTS
    void Start()
    {
        _cameraZ = transform.position.z;
        _player         = GameObject.FindObjectOfType<Player>();
        if (_player == null)
            Debug.LogError("Player Object can no be found");

        _originalParent = transform.parent; //set the original parent
    }

    void LateUpdate()
    {
        if (_player.isDead)
            return;

        float distanceToPlayer = Mathf.Abs(transform.position.y - _player.transform.position.y) ;

        if (distanceToPlayer <= _stoppingDistance)
            return;



        Vector2 targetPos = _player.transform.position;
        targetPos += Vector2.up * _distance;

        Vector2 newPos = Vector2.Lerp(transform.position , targetPos , Time.deltaTime * _smooth * (distanceToPlayer < 1.0f? 1.0f : distanceToPlayer));

        transform.position = new Vector3(newPos.x , newPos.y , _cameraZ);

       
    }
   

}
