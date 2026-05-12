using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerCollisionController))]
    public class PlayerAnimController : MonoBehaviour
    {
        public float FacingDirection { get; private set; }

        private Animator animator;
        private Rigidbody2D rb;
        private PlayerCollisionController collisionController;

        private float xVelocity;
        private float yVelocity;
        private bool isGrounded;
        private bool isHurt;

        private static readonly int MovingAnimParam = Animator.StringToHash("xVelocity");
        private static readonly int JumpAnimParam = Animator.StringToHash("yVelocity");
        private static readonly int IsGroundedAnimParam = Animator.StringToHash("isGrounded");
        private static readonly int WallGrabParam = Animator.StringToHash("wallDetected");
        private static readonly int HurtAnimParam = Animator.StringToHash("isHurt");

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
            HandleHurtAnimEvent(isHurt);
        }

        private void OnEnable() => PlayerHealthController.OnInvincibilityChanged += PlayerIsHurt;
        private void OnDisable() => PlayerHealthController.OnInvincibilityChanged -= PlayerIsHurt;

        private void HandleFacingDirection(float horizontalVelocity)
        {
            if (horizontalVelocity > 0.01f)
            {
                transform.localScale = new Vector3(1, 1, 1);
                FacingDirection = 1f;
            }
            else if (horizontalVelocity < -0.01f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                FacingDirection = -1f;
            }
        }

        private void PlayerIsHurt(bool isInvincible) => isHurt = isInvincible;

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

        private void HandleHurtAnimEvent(bool isInvincible) => animator.SetBool(HurtAnimParam, isInvincible);
    }
}
