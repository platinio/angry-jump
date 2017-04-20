using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    #region PUBLIC_FIELDS
    [Header("Player Settings")]
	public float jumpForce;
	[Header("RayCasting Settings")]
	public float rayLegth;
	public float offSetX;
	public float offSetY;
	public LayerMask collisionLayer;
    #endregion PUBLIC_FIELDS

    #region PRIVATE_FIELDS
    private Rigidbody2D RB;
	private Animator anim;
    private float initialHeight;
    private bool checkHeight;
    private bool IsGrounded
	{
		get
		{
			return Physics2D.Raycast (new Vector2(transform.position.x + offSetX , transform.position.y + offSetY) , Vector2.down , rayLegth , collisionLayer);		
		}
	}
    #endregion PRIVATE_FIELDS


    public void Initialize()
    {
        //get references
        RB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        initialHeight = transform.position.y;
    }


    void Update()
    {
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
    /*
    private void SetAnimParam()
    {
        anim.SetBool("IsGrounded", IsGrounded);
        anim.SetBool("IsFalling", RB.velocity.y < 0.1);
        anim.SetBool("IsJumping", RB.velocity.y > 0.1);
    }
    */
    //
    void OnDrawGizmosSelected()
    {
        Debug.DrawLine(new Vector2(transform.position.x + offSetX, transform.position.y + offSetY), new Vector2(transform.position.x + offSetX, transform.position.y + offSetY - rayLegth), Color.red);

    }


}
