using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(PlayerCollisionController))]
    public class PlayerMovementController : MonoBehaviour
    {
        public bool IsKnockedBack { get => isKnockedback; set => isKnockedback = value; }

        [Header("References")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float runMultiplier = 2f;
        [SerializeField] private float wallSlideSpeed = 1f;
        [SerializeField] private bool isSprintingPressed;

        private Rigidbody2D rb;
        private PlayerInputHandler inputHandler;
        private PlayerStats playerStats;
        private PlayerCollisionController collisionController;
        private Vector2 moveInput;
        private bool isKnockedback;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            inputHandler = GetComponent<PlayerInputHandler>();
            playerStats = GetComponent<PlayerStats>();
            collisionController = GetComponent<PlayerCollisionController>();
        }

        private void Update()
        {
            if (isKnockedback) return;
            
            HandleWallSlide();

            if (!collisionController.IsGrounded) return;

            moveInput = inputHandler.GetMoveInput();
            isSprintingPressed = inputHandler.IsSprintPressed();

            HandleMovement(moveInput);
            HandleSprinting(isSprintingPressed);
        }

        private void HandleWallSlide()
        {
            if (!collisionController.IsTouchingWall || collisionController.IsGrounded) return;
            if (rb.linearVelocity.y >= 0) return;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        }

        private void HandleMovement(Vector2 moveDirection)
        {
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
        }

        private void HandleSprinting(bool sprinting)
        {
            bool canActuallySprint = sprinting && playerStats.CanSprint() && moveInput.x != 0; 
        
            playerStats.SpendStamina(canActuallySprint);
        
            if (canActuallySprint)
            {
                rb.linearVelocity = new Vector2(moveInput.x * moveSpeed * runMultiplier, rb.linearVelocity.y);
            }
        }
    }
}
