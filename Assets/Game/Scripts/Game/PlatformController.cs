using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformController : MonoBehaviour
{

    public float speed;
    public bool isVertical;
    public float distanceToReset;   
    [HideInInspector] public bool isConnected;
    [HideInInspector] public float height;
    
    [HideInInspector] public Rigidbody2D RB;
    private SpriteRenderer SP;  
   

    public void Initialize()
    {
        RB = GetComponent<Rigidbody2D>();

        if (RB != null)
            RB.isKinematic = true; //deactivated physics

        Collider2D col = GetComponent<Collider2D>();

        if (isVertical)
            height = col.bounds.size.x;        
        else
            height = col.bounds.size.y;            
        

    }



    void Update()
    {

        if (transform.position.y < GameManager.instance.initialPlatform.position.y)
            Destroy(gameObject , 3.0f);
        
        if (speed == 0)
            return;
        if(!isVertical)
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        else
            transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag(Constants.instance.tags.platform))
        {
            if (!isConnected)
                isConnected = !isConnected;           
        }

        if (speed == 0)
            return;

        if (RB == null)
            return;
        

        if (other.gameObject.CompareTag(Constants.instance.tags.player))
        {
            RB.isKinematic = false; //reactivated physics
            speed = 0;
            GameManager.instance.UpdateHeight(height);
            Invoke("CreatePlatform", Constants.instance.values.createPlatformDelay);
            //GameManager.instance.CreateRandomPiece();//reate another piece
        }

        
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Constants.instance.tags.platform))
        {
            if (isConnected)
                isConnected = !isConnected;
           
        }
    }

    private void CreatePlatform()
    {
        GameManager.instance.CreateRandomPiece();
    }
        
	
}
