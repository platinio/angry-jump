using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float speed;
    public bool isVertical;
    [HideInInspector]
    public float height;

    private Rigidbody2D RB;
    private SpriteRenderer SP;
    
   

    public void Initialize()
    {
        RB = GetComponent<Rigidbody2D>();

        if (RB != null)
            RB.isKinematic = true;

        if (isVertical)
        {
            //do soemting
        }
        else
        {
            height = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        }

    }


    void Update()
    {
        if(speed != 0)
            transform.Translate(Vector2.up * speed * Time.deltaTime);
               

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (speed == 0)
            return;

        if (RB == null)
            return;
        

        if (other.gameObject.CompareTag("Player"))
        {
            RB.isKinematic = false;
            speed = 0;
            GameManager.instance.UpdateHeight(height);
            GameManager.instance.CreateRandomPiece();
        }
    }
	
}
