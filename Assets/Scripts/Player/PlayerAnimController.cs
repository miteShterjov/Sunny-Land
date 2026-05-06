using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerCollisionController))]
    public class PlayerAnimController : MonoBehaviour
    {
        private Animator animator;
        private Rigidbody2D rb;
        private PlayerCollisionController collisionController;

        private float xVelocity;
        private float yVelocity;
        private bool isGrounded;
    
        private static readonly int MovingAnimParam = Animator.StringToHash("xVelocity");
        private static readonly int JumpAnimParam = Animator.StringToHash("yVelocity");
        private static readonly int IsGroundedAnimParam = Animator.StringToHash("isGrounded");
        private static readonly int WallGrabParam = Animator.StringToHash("wallDetected");

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody2D>();
            collisionController = GetComponent<PlayerCollisionController>();
        }

        private void Update()
        {
            isGrounded = collisionController.IsGrounded;
            xVelocity = rb.linearVelocity.x;
            yVelocity = rb.linearVelocity.y;

            animator.SetBool(IsGroundedAnimParam, isGrounded);

            HandleWallGrabAnimEvent();
            HandleMovingAnimEvent();
            HandleJumpingAnimEvent();
            if (!collisionController.IsTouchingWall || isGrounded) HandleFacingDirection(xVelocity);
        }

        private void HandleFacingDirection(float horizontalVelocity)
        {
            transform.localScale = horizontalVelocity switch
            {
                > 0.01f => new Vector3(1, 1, 1),
                < -0.01f => new Vector3(-1, 1, 1),
                _ => transform.localScale
            };
        }
        
        public void Flip() => transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        private void HandleMovingAnimEvent() => animator.SetFloat(MovingAnimParam, Mathf.Abs(xVelocity));
        
        private void HandleJumpingAnimEvent() => animator.SetFloat(JumpAnimParam, yVelocity);

        private void HandleWallGrabAnimEvent()
        {
            bool wallGrabActive = collisionController.IsTouchingWall && !isGrounded;
            animator.SetBool(WallGrabParam, wallGrabActive);

            if (!wallGrabActive) return;

            // Face toward the wall (set directly, never call Flip() per-frame)
            if (collisionController.IsWallRight)
                transform.localScale = new Vector3(1, 1, 1);
            else if (collisionController.IsWallLeft)
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
