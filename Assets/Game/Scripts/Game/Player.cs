using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    #region PUBLIC_FIELDS

    public int frameGroundedUpdate;

    [Header("Player Settings")]
	public float jumpForce;
    public float rayLength;

    [Header("RayCasting 1 Settings")]
    public float offSetX;
    public float offSetY;
    
    [Header("RayCasting 2 Settings")]
    public float _offSetX;
    public float _offSetY;

    public Vector2 LastKnowPos
    {
        get { return lastKnowPos; }
        private set { lastKnowPos = value; }
    }

    #endregion PUBLIC_FIELDS

    #region PRIVATE_FIELDS
    private int currentFrame;
    private Vector2 lastKnowPos;
    private Rigidbody2D RB;
	private Animator anim;
    private GameObject currentPlatform;
    private float initialHeight;
    private bool checkHeight;
    private bool IsGrounded
	{
		get
		{
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + offSetX , transform.position.y + offSetY), Vector2.down, rayLength, Constants.instance.layers.Platform);
            RaycastHit2D _hit = Physics2D.Raycast(new Vector2(transform.position.x + _offSetX, transform.position.y + _offSetY), Vector2.down, rayLength, Constants.instance.layers.Platform);

          

            if (hit.collider == null && _hit.collider == null)
                return false;

            
            GameObject platform = hit.collider == null ? _hit.collider.gameObject : hit.collider.gameObject;

            if (currentPlatform != null)
            {
                if (currentPlatform != platform)
                {
                    currentPlatform = platform;
                    UIManager.instance.AddScore();
                }
            }

            else
            {
                currentPlatform = platform;
                //UIManager.instance.AddScore();
            }

            
            

			return true;		
		}
	}
    #endregion PRIVATE_FIELDS

    void Awake()
    {
        lastKnowPos = transform.position;
    }
    public void Initialize()
    {
        
        //get references
        RB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        initialHeight = transform.position.y;
    }


    void Update()
    {
        currentFrame++;
        if (currentFrame > frameGroundedUpdate)
        {
            if (IsGrounded)
            {
                //update last know position
                lastKnowPos = transform.position;
                currentFrame = 0;
            }
        }
            

        //if we are running on a pc, web or testing from editor
        #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        if (Input.GetButtonDown("Jump"))
            Jump();
        //else if we are runnin on a phone
        #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        if (Input.touchCount > 0)
        {
            //Store the first touch detected.
            Touch myTouch = Input.touches[0];

            //Check if the phase of that touch equals Began
            if (myTouch.phase == TouchPhase.Began)
                Jump();
            
        }
        #endif


        //SetAnimParam();
        //if we fall
        if (transform.position.y < (initialHeight - 1))
            GameManager.instance.GameOver();

       

        SetAnimParam();
    }

    void FixedUpdate()
    {
        if (RB.velocity.x != 0)
        {
            Vector2 newVelocity = RB.velocity;
            newVelocity.x = 0;
            RB.velocity = newVelocity;
        }
    }

    /// <summary>
    /// Make the player jump
    /// </summary>
    public void Jump()
    {
       //if we are no touching the ground return
        if (!IsGrounded)
            return;

        _Jump();
    }

    /// <summary>
    /// force a jump witout any check
    /// </summary>
    public void ForceJump()
    {
        _Jump();
    }

    /// <summary>
    /// implementation of the jump
    /// </summary>
    private void _Jump()
    {
        RB.velocity = new Vector2(0, 0);
        RB.AddForce(new Vector2(0, jumpForce));                
    }

    

    public void Kill()
    {
        //GameManager.instance.GameOver();
    }
    
    private void SetAnimParam()
    {
        anim.SetBool("Idle", IsGrounded);
        anim.SetBool("Fall", RB.velocity.y < 0.1);
        anim.SetBool("Jump", RB.velocity.y > 0.1);
    }
    
    //
    void OnDrawGizmosSelected()
    {
        //draw ray 1
        Debug.DrawLine(new Vector2(transform.position.x + offSetX , transform.position.y + offSetY), new Vector2(transform.position.x + offSetX , transform.position.y + offSetY - rayLength), Color.red);
        //draw ray 2
        Debug.DrawLine(new Vector2(transform.position.x + _offSetX , transform.position.y + _offSetY), new Vector2(transform.position.x + _offSetX , transform.position.y + _offSetY - rayLength), Color.blue);


    }


}
