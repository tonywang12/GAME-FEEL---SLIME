using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoftBodyControls : MonoBehaviour
{   

    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 moveInput;
    public float acceleration;
    public float decceleration;
    public float velPower;

    public Transform groundCheck;
   public LayerMask groundLayer;
   public float groundCheckSize = 0.2f;
       [SerializeField] private bool onGround = true;

    bool facingRight = true;

    public Rigidbody2D mainBody;
    Rigidbody2D[] componentBodies;

    public float jumpPower;
private float coyoteTimeCounter;
   public float coyoteTime;
   private float jumpBufferCounter;
   public float jumpBufferTime;


    public ParticleSystem trail;
    public GameObject trailPos;
 


    [Header("Physics")]
    //public float maxSpeed = 7f;
    public float linearDrag = 4f;

    void Start()
    {
        componentBodies = GetComponentsInChildren<Rigidbody2D>();
    }

    void Update()
    {   
        trailPos.transform.position = new Vector3 (transform.position.x, transform.position.y, 1);
        //Debug.Log(onGround);
        onGround = IsGrounded();

    if (onGround)
     {    
          coyoteTimeCounter = coyoteTime;
     } else
     {
          coyoteTimeCounter -= Time.deltaTime;
     }

    //If jump isn't pressed -- jumpbuffer to check if jump is able to do in time
     if (!Gamepad.current.buttonSouth.wasPressedThisFrame && !Keyboard.current.spaceKey.wasPressedThisFrame)
     {
          jumpBufferCounter -= Time.deltaTime;
     }
    }

    void FixedUpdate()
    {   
        modifyPhysics();

        //get the inputed desired speed
        float targetSpeed = moveInput.x * moveSpeed;
        //calculate the desired speed with current velocity
        float speedDif = targetSpeed - mainBody.velocity.x;
        //add acceleration based on targetspeed situation
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        foreach(var rb in componentBodies)
        {
            rb.AddForce(Vector2.right * moveInput.x * moveSpeed);
        } 

        if(moveInput.x > 0 && !facingRight)
        {
           //Flip();
        }

        if(moveInput.x < 0 && facingRight)
        {
            //Flip();
        }

        if(moveInput.x > 0.3 || moveInput.x < -0.3 )
        {
           //CreateTrail();
        }

    

    }
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }


    public void Jump(InputAction.CallbackContext context)
    {       
        
        Debug.Log("jump!");
        if (context.performed)
        {
          jumpBufferCounter = jumpBufferTime;
        }    

        //Check if Coyote Time is avaliable for an jump to occur
        //Check if jump buffer counter is able to perform a jump
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f) //Jump Pressed + Player is on ground layer
        {   
            if(onGround)
            {
            Jumping();
          //rb.velocity += Vector2.up * jumpForce;
          //reset counters after performing jump
          coyoteTimeCounter = 0f;
          jumpBufferCounter = 0f;
            }
        }
    } 

    void Jumping(){
        foreach(var rb in componentBodies)
        {
            rb.AddForce(Vector2.up * jumpPower);
        }
        //gameObject.GetComponent<Animator>().SetTrigger("Stretch");
        CreateTrail();
        SoundManager.PlaySound("jump");
    }

    private bool IsGrounded()
   {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckSize, groundLayer);
   }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        
        facingRight = !facingRight;
    }

    void modifyPhysics()
   {    
        bool changeDir = (moveInput.x > 0 && mainBody.velocity.x < 0) || (moveInput.x < 0 && mainBody.velocity.x > 0);

        if(onGround)
        {
            if(Mathf.Abs(moveInput.x) < 0.4f || changeDir)
            {
                mainBody.drag = linearDrag;
            }else
            {
                mainBody.drag = 0;
            }
            
        }
   }

   void CreateTrail(){
     trail.Play();
   }
}
