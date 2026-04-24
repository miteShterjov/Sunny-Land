using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 15f;
    
    [Header("Double Jump")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool canDoubleJump = true;
    [SerializeField ] private bool hasDoubleJump = true;

    private Rigidbody2D rb;
    private PlayerInputHandler inputHandler;
    private PlayerCollisionController collisionController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<PlayerInputHandler>();
        collisionController = GetComponent<PlayerCollisionController>();
    }

    private void Update()
    {
        isGrounded = collisionController.IsGrounded;
        canDoubleJump = !isGrounded && hasDoubleJump;
        
        if (isGrounded || hasDoubleJump) HandleInput();
        if (isGrounded && !hasDoubleJump) ResetDoubleJump();
    }

    private void HandleInput()
    {
        if (inputHandler.IsJumpPressed())
        {
            if (isGrounded || canDoubleJump) TryJump();
        }
    }

    private void TryJump()
    {
        Jump();
        if (!isGrounded) hasDoubleJump = false;
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void ResetDoubleJump()
    {
        if (collisionController.JustGrounded) hasDoubleJump = true;
    }
}
 