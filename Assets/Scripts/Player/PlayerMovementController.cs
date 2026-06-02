using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerData))]
    [RequireComponent(typeof(PlayerCollisionController))]
    public class PlayerMovementController : MonoBehaviour
    {
        public bool IsKnockedBack { get => isKnockedback; set => isKnockedback = value; }

        [Header("References")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float sprintMultiplier = 2f;
        [SerializeField] private float wallSlideSpeed = 1f;
        [SerializeField] private float climbSpeed = 3f;
        [SerializeField] private bool isSprintingPressed;

        private Rigidbody2D rb;
        private PlayerInputHandler inputHandler;
        private PlayerData playerStats;
        private PlayerCollisionController collisionController;
        private Vector2 moveInput;
        private bool isKnockedback;
        private float originalGravityScale;
        private bool isClimbing;
        private bool canSprint;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            inputHandler = GetComponent<PlayerInputHandler>();
            playerStats = GetComponent<PlayerData>();
            collisionController = GetComponent<PlayerCollisionController>();
            originalGravityScale = rb.gravityScale;
        }

        private void Update()
        {
            if (isKnockedback) return;

            moveInput = inputHandler.GetMoveInput();
            isSprintingPressed = inputHandler.IsSprintPressed();
            isClimbing = collisionController.IsOnStairs && inputHandler.GetClimbInput() != 0;

            if (isClimbing) return;
            if (!collisionController.IsGrounded) return;
    
            // just calculate CAN sprint, don't set velocity here
            canSprint = isSprintingPressed && playerStats.CanSprint() && moveInput.x != 0;
            playerStats.SpendStamina(canSprint);
        }

        private void FixedUpdate()
        {
            rb.gravityScale = isClimbing ? 0f : originalGravityScale;

            if (isClimbing) HandleClimbing();
            else HandleMovement(moveInput, canSprint); // pass sprint state
    
            if (!isClimbing) HandleWallSlide();
        }

        private void HandleMovement(Vector2 moveDirection, bool sprinting = false)
        {
            float speed = sprinting ? moveSpeed * sprintMultiplier : moveSpeed;
            rb.linearVelocity = new Vector2(moveDirection.x * speed, rb.linearVelocity.y);
        }
        
        private void HandleClimbing()
        {
            float climbInput = inputHandler.GetClimbInput();
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed * 0.5f, climbInput * climbSpeed);
        }
        
        private void HandleWallSlide()
        {
            if (!collisionController.IsTouchingWall || collisionController.IsGrounded) return;
            if (rb.linearVelocity.y >= 0) return;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        }
    }
}
