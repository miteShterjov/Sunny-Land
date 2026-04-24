using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerCollisionController collisionController;

    private float xVelocity;
    private float yVelocity;
    
    private static readonly int MovingAnimParam = Animator.StringToHash("xVelocity");
    private static readonly int JumpAnimParam = Animator.StringToHash("yVelocity");
    private static readonly int IsGroundedAnimParam = Animator.StringToHash("isGrounded");

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collisionController = GetComponent<PlayerCollisionController>();
    }

    void Update()
    {
        xVelocity = rb.linearVelocity.x;
        yVelocity = rb.linearVelocity.y;

        animator.SetBool(IsGroundedAnimParam, collisionController.IsGrounded);
        

        HandleMovingAnimEvent(xVelocity);
        HandleJumpingAnimEvent(yVelocity);
        HandleFacingDirection(xVelocity);
    }

    private void HandleFacingDirection(float xVelocity)  
    {  
        if (xVelocity > 0.01f) transform.localScale = new Vector3(1, 1, 1);
        else if (xVelocity < -0.01f) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleMovingAnimEvent(float  xVelocity) => animator.SetFloat(MovingAnimParam, Mathf.Abs(xVelocity));
    private void HandleJumpingAnimEvent(float  yVelocity) => animator.SetFloat(JumpAnimParam, yVelocity);
}
