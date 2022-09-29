using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{   
    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 moveInput;
   public float acceleration;
   public float decceleration;
   public float velPower;

    [SerializeField] private bool facingRight = true;
    [Header("Jump Variables")]
    public float jumpForce = 10f;
    public Transform groundCheck;
   public LayerMask groundLayer;
   public float groundCheckSize = 0.2f;
    [SerializeField] private bool onGround = true;
    public float gravity = 1f;
   public float fallMultiplier = 2.5f;

    private float coyoteTimeCounter;
   public float coyoteTime;
   private float jumpBufferCounter;
   public float jumpBufferTime;
    [Header("Player Components")]
        public Rigidbody2D rb;

    [Header("Physics")]
    //public float maxSpeed = 7f;
    public float linearDrag = 4f;

    void Update()
    {
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
     float speedDif = targetSpeed - rb.velocity.x;
     //add acceleration based on targetspeed situation
     float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

     float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
     
     movePlayer(moveInput.x);
    }

    public void Move(InputAction.CallbackContext context)
   {    
        //read input of player
        moveInput = context.ReadValue<Vector2>();
   }    

   void movePlayer(float horizontal)
   {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);

        if(horizontal > 0 && !facingRight || (horizontal < 0 && facingRight))
        {
            //Flip();
        }
        // if(Mathf.Abs(rb.velocity.x) > maxSpeed)
        // {
        //     rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        // }
   }

   public void Jump(InputAction.CallbackContext context)
   {    
    //when jump is pressed, a buffer is placed
     if (context.performed)
     {
          jumpBufferCounter = jumpBufferTime;
     }  

     //Check if Coyote Time is avaliable for an jump to occur
     //Check if jump buffer counter is able to perform a jump
     if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f) //Jump Pressed + Player is on ground layer
     {    
            Jumping();
          //rb.velocity += Vector2.up * jumpForce;
          //reset counters after performing jump
          coyoteTimeCounter = 0f;
          jumpBufferCounter = 0f;
     }
     
    }
   

   void Jumping(){
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
   }

    private bool IsGrounded()
   {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckSize, groundLayer);
   }
   void modifyPhysics()
   {    
        bool changeDir = (moveInput.x > 0 && rb.velocity.x < 0) || (moveInput.x < 0 && rb.velocity.x > 0);

        if(onGround)
        {
            if(Mathf.Abs(moveInput.x) < 0.4f || changeDir)
            {
                rb.drag = linearDrag;
            }else
            {
                rb.drag = 0;
            }
            rb.gravityScale = 0;
        }else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if(rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }else if(rb.velocity.y > 0 && !Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
   }
   void Flip()
   {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 100, 0);
   }
}