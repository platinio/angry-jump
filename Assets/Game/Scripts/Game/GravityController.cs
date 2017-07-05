using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour 
{
    [SerializeField] private float _rot                 = 0.0f;
    [SerializeField] private float _minAngle            = 30.0f;
    [SerializeField] private float _maxAngle            = 300.0f;
    [SerializeField] private float _maxGravitySpeed     = 12.0f;
    
    private float _gravitySpeed     = 0.0f;
    private float _speed            = 0.0f;

    void Start()
    {
        _speed = _maxAngle / _maxGravitySpeed;

        Vector2 gravity = Physics2D.gravity;
        gravity.x = 0;
        Physics2D.gravity = gravity;
    }


    void Update()
    {
        _rot = Input.acceleration.x * 360.0f;

        //if we are in a normal range
        if (Mathf.Abs(_rot) < _minAngle)
        {
            if (Physics2D.gravity.x != 0)
            {
                Vector2 gravity = Physics2D.gravity;
                gravity.x = 0;
                Physics2D.gravity = gravity;
            }

            return;
        }


        _gravitySpeed = _rot / _speed;
        
        if (_gravitySpeed < -_maxGravitySpeed)
            _gravitySpeed = -_maxGravitySpeed;
        else if (_gravitySpeed > _maxGravitySpeed)
            _gravitySpeed = _maxGravitySpeed;
        
        Physics2D.gravity = new Vector2(_gravitySpeed , Physics2D.gravity.y);
    }
}
