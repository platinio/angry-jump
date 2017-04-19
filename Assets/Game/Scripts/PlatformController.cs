using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D RB;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();

        if (RB != null)
            RB.isKinematic = true;
    }


    void Update()
    {
        if(speed != 0)
            transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("entro");

        if (RB == null)
        {
            return;
        }
            

        if (other.gameObject.CompareTag("Player"))
        {
            RB.isKinematic = false;
            speed = 0;
        }
    }
	
}
