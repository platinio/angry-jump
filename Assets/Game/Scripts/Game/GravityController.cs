using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour 
{
    public float rot;
    public float minAngle;
    public float maxAngle;
    public float maxGravitySpeed;
    public float gravitySpeed;

    private float speed;

    void Start()
    {
        speed = maxAngle / maxGravitySpeed;
    }


    void Update()
    {
        rot = Input.acceleration.x * 360.0f;

        if (Mathf.Abs(rot) < minAngle)
            return;

        gravitySpeed = rot / speed;
        
        if (gravitySpeed < -maxGravitySpeed)
            gravitySpeed = -maxGravitySpeed;
        else if (gravitySpeed > maxGravitySpeed)
            gravitySpeed = maxGravitySpeed;
        
        Physics2D.gravity = new Vector2(gravitySpeed , Physics2D.gravity.y);
    }
}
