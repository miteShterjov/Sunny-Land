using System;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runMultiplier = 2f;
    [SerializeField] private float airControlPercent = 0.7f; // Reduced control in air
    [SerializeField] private bool isSprintingPressed;

    private Rigidbody2D rb;
    private PlayerInputHandler inputHandler;
    private PlayerStats playerStats;
    private PlayerJumpController jumpController;
    private PlayerCollisionController collisionController;

    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<PlayerInputHandler>();
        playerStats = GetComponent<PlayerStats>();
        jumpController = GetComponent<PlayerJumpController>();
        collisionController = GetComponent<PlayerCollisionController>();
    }

    private void Update()
    {
        moveInput = inputHandler.GetMoveInput();
        isSprintingPressed = inputHandler.IsSprintPressed();
        
        HandleMovement(moveInput);
        HandleSprinting(isSprintingPressed);
        
    }

    private void HandleMovement(Vector2 moveDirection)
    {
        SetLinearVelocity(moveDirection * moveSpeed);
    }

    private void HandleSprinting(bool sprinting)
    {
        bool canActuallySprint = sprinting && playerStats.CanSprint() && moveInput.x != 0; 
        
        playerStats.SpendStamina(canActuallySprint);
        
        if (canActuallySprint)
        {
            SetLinearVelocity(moveSpeed * runMultiplier * moveInput);
        }
    }

    private void SetLinearVelocity(Vector2 velocity) => rb.linearVelocity = velocity;
   
    private void SetLinearVelocity(float x, float y) => rb.linearVelocity = new Vector2(x, y);
}
