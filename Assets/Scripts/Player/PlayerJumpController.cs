using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerData))]
    [RequireComponent(typeof(PlayerCollisionController))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerJumpController : MonoBehaviour
    {
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 15f;
        [SerializeField] private float doubleJumpForce = 10f;
        [SerializeField] private float jumpStaminaCost = 5f;
        [SerializeField] private Vector2 wallJumpForce;
        [SerializeField] private bool canDoubleJump = true;

        private bool isGrounded;
        private bool jumpIsPressed;

        private Rigidbody2D rb;
        private PlayerData playerStats;
        private PlayerCollisionController collisionController;
        private PlayerInputHandler inputHandler;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerStats = GetComponent<PlayerData>();
            collisionController = GetComponent<PlayerCollisionController>();
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            isGrounded = collisionController.IsGrounded;
            jumpIsPressed = inputHandler.IsJumpPressed();

            if (playerStats.CurrentStamina <= 0) return;
            if (isGrounded && !canDoubleJump) ResetDoubleJump();

            switch (jumpIsPressed)
            {
                case true when isGrounded:
                    Jump(jumpForce);
                    break;
                case true when collisionController.IsTouchingWall && !isGrounded:
                    WallJump();
                    break;
                case true when !isGrounded && canDoubleJump:
                    Jump(doubleJumpForce);
                    canDoubleJump = false;
                    break;
            }
        }

        private void Jump(float force)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            playerStats.SpendStamina(jumpStaminaCost);
        }

        private void WallJump()
        {
            float xDir = collisionController.IsWallRight ? -1f : 1f;
            rb.linearVelocity = new Vector2(wallJumpForce.x * xDir, wallJumpForce.y);
            canDoubleJump = true;
            playerStats.SpendStamina(jumpStaminaCost);
        }

        private void ResetDoubleJump() => canDoubleJump = true;
    }
}
 