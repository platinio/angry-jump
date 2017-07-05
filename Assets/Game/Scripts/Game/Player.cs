using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
   

    //inspector
    [SerializeField] private float _deadAnimSpeed   = 5.0f;
    [SerializeField] private float _deadTime        = 0.6f;

    [Header("Player Settings")]
    [SerializeField] private float _jumpForce       = 450.0f;
    [SerializeField] private float _rayLength       = 0.47f;

    [Header("RayCasting 1 Settings")]
    [SerializeField] private float _offSetX         = 0.24f;
    [SerializeField] private float _offSetY         = -0.63f;
    
    //how fast we update the IsGrounded
    private int frameGroundedUpdate = 5;

    
    #region PRIVATE_FIELDS
    private float           _maxFallSpeed       = 6.3f;
    private int             _currentFrame       = 0;
    private Vector2         _lastKnowPos        = Vector2.zero;
    private Rigidbody2D     _RB                 = null;
	private Animator        _anim               = null;
    private GameObject      _currentPlatform    = null;
    private float           _initialHeight      = 0.0f;
    private bool            _dead               = false;
    private bool            _checkHeight        = false;
    
    public bool isDead { get { return _dead; } set { _dead = value; } }

    private bool IsGrounded
	{
		get
		{
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + _offSetX , transform.position.y + _offSetY), Vector2.left, _rayLength, Constants.instance.layers.Platform);
                                  

            if (hit.collider == null)
                return false;


            GameObject platform = hit.collider.gameObject;

            if (_currentPlatform != null)
            {
                if (_currentPlatform != platform)
                {
                    _currentPlatform = platform;
                    UIManager.instance.AddScore();
                }
            }

            else
            {
                _currentPlatform = platform;
                //UIManager.instance.AddScore();
            }

           
			return true;		
		}
	}
    #endregion PRIVATE_FIELDS

    public Vector2 LastKnowPos {  get { return _lastKnowPos; } private set { _lastKnowPos = value; }}    

    void Awake()
    {
        _lastKnowPos = transform.position;
    }
    public void Initialize()
    {        
        //get references
        _RB             = GetComponent<Rigidbody2D>();
        _anim           = GetComponent<Animator>();
        _initialHeight  = transform.position.y;
    }


    void Update()
    {
        //if we are dead return
        if (_dead)
            return;
        //if the game if over return
        if (GameManager.instance.GameState == GameState.GameOver)
            return;

        //update last know position
        _currentFrame++;
        if (_currentFrame > frameGroundedUpdate)
        {
            if (IsGrounded)
            {
                //update last know position
                _lastKnowPos = transform.position;
                _currentFrame = 0;
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
        if (transform.position.y < (_initialHeight - 1))
        {
            GameManager.instance.GameOver();
            _dead = true;
        }
            

       

        SetAnimParam();
    }

    void FixedUpdate()
    {
        //unable physics on horizontal forces
        if (_RB.velocity.x != 0)
        {
            Vector2 newVelocity = _RB.velocity;
            newVelocity.x = 0;
            _RB.velocity = newVelocity;
        }

        //check falling speed
        if(_RB.velocity.y < -_maxFallSpeed)
        {
            Vector2 newVelocity = _RB.velocity;
            newVelocity.y = -_maxFallSpeed;
            _RB.velocity = newVelocity;
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
        SoundManager.instance.PlayJumpSound();

        _RB.velocity = new Vector2(0, 0);
        _RB.AddForce(new Vector2(0, _jumpForce));                
    }

   
    public void Kill(int dir)
    {
        SoundManager.instance.PlayHitSound();
        StartCoroutine(CO_Kill(dir));            
    }

    IEnumerator CO_Kill(int dir)
    {
        
        float timer = 0;
        while (timer < _deadTime)
        {

            
            transform.Translate(Vector2.right * dir * _deadAnimSpeed * Time.deltaTime);
            timer += Time.deltaTime;

            if (_dead)
                timer = _deadTime;

            yield return new WaitForEndOfFrame();
        }
        
    }
    
    private void SetAnimParam()
    {
        _anim.SetBool("Idle", IsGrounded);
        _anim.SetBool("Fall", _RB.velocity.y < 0.1);
        _anim.SetBool("Jump", _RB.velocity.y > 0.1);
    }
    
    //
    void OnDrawGizmosSelected()
    {
        //draw ray 1
        Debug.DrawLine(new Vector2(transform.position.x + _offSetX , transform.position.y + _offSetY), new Vector2(transform.position.x + _offSetX - _rayLength, transform.position.y + _offSetY), Color.red);
      

    }


}
