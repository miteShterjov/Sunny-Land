using System;
using Enemies;
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
        // [SerializeField] private bool canDoubleJump = true;

        private bool isGrounded;
        private bool hasDoubleJump;
        private bool wasJumpPressedLastFrame;
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
            bool jumpPressedThisFrame = inputHandler.IsJumpPressed();

            // BUG 2 FIX: only act on the frame the button is first pressed
            bool jumpTriggered = jumpPressedThisFrame && !wasJumpPressedLastFrame;
            wasJumpPressedLastFrame = jumpPressedThisFrame;

            // BUG 3 FIX: restore double jump clearly when landing
            if (isGrounded) hasDoubleJump = true;

            if (playerStats.CurrentStamina <= 0) return;
            if (!jumpTriggered) return;

            if (isGrounded)
            {
                Jump(jumpForce);
            }
            else if (collisionController.IsTouchingWall)
            {
                WallJump();     // BUG 4 PARTIAL FIX: jump controller owns this decision
            }
            else if (hasDoubleJump)
            {
                Jump(doubleJumpForce);
                hasDoubleJump = false;
            }
        }

        private void OnEnable() => Enemy.OnEnemyStomped += BounceOffEnemy;

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
            hasDoubleJump = true;
            playerStats.SpendStamina(jumpStaminaCost);
        }
        
        private void BounceOffEnemy() => rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        // private void ResetDoubleJump() => hasDoubleJump = true;
    }
}
