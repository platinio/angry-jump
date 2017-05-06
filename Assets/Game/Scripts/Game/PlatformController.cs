using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformController : MonoBehaviour
{
    public float speed;
    public bool isVertical;
    [HideInInspector]
    public float height;
    public Transform Center
    {
        get
        {
            if (_Center == null)
                _Center = transform.FindChild("Center");
            return _Center;
        }

        set
        {
            _Center = value;
        }
    }

    private Transform _Center;
    private Rigidbody2D RB;
    private SpriteRenderer SP;
    
   

    public void Initialize()
    {
        RB = GetComponent<Rigidbody2D>();

        if (RB != null)
            RB.isKinematic = true;

        if (isVertical)
        {
            Collider2D col = GetComponent<Collider2D>();
            height = col.bounds.size.x * 2;

        }
        else
        {
            Collider2D col = GetComponent<Collider2D>();
            height = col.bounds.size.y;
        }

    }


    void Update()
    {
        if (speed == 0)
            return;
        if(!isVertical)
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        else
            transform.Translate(Vector2.down * speed * Time.deltaTime);

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
    
    
    public void SetPositionTop()
    {
        
    }
	
}
