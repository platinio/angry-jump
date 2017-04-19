using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour 
{
    [Header("Player Settings")]
	public float jumpForce;

	[Header("RayCasting Settings")]
	public float rayLegth;
	public float offSetX;
	public float offSetY;
	public LayerMask collisionLayer;
	
	private Rigidbody2D RB;
	private Animator anim;
    private bool checkHeight;

	private bool IsGrounded
	{
		get
		{
			return Physics2D.Raycast (new Vector2(transform.position.x + offSetX , transform.position.y + offSetY) , Vector2.down , rayLegth , collisionLayer);		
		}

	}

    public void Initialize()
    {
        RB = GetComponent<Rigidbody2D>();

        if (!RB)
        {
            Debug.LogError("No rigidbody in player, please add one");
        }

        anim = GetComponent<Animator>();

        if (!anim)
        {
            Debug.LogError("No animator in player, please add one");
        }


    }


    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            Jump();
        //SetAnimParam();

                
    }

    public void Jump()
    {
       //si no esta en el suelo retornamos
        if (!IsGrounded)
            return;

        _Jump();
    }

    public void ForceJump()
    {
        _Jump();
    }

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
