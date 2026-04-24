using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    [Header("Ground Detection")]
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    [Header("Wall Detection")]
    [SerializeField] private float wallCheckDistance = 0.1f;
    [SerializeField] private LayerMask wallLayer;
    [Header("Collision States")]
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool wasGrounded = false;
    [SerializeField] private bool isWallLeft = false;
    [SerializeField] private bool isWallRight = false;
    
    private CapsuleCollider2D capsuleCollider;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        UpdateGroundDetection();
        UpdateWallDetection();
    }

    private void UpdateGroundDetection()
    {
        // Store previous state
        wasGrounded = isGrounded;
        
        // Raycast down from collider bottom
        Vector2 raycastOrigin = (Vector2)transform.position + Vector2.down * (capsuleCollider.size.y / 2);
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, groundCheckDistance, groundLayer);

        isGrounded = hit.collider != null;
    }

    private void UpdateWallDetection()
    {
        // Raycast left from collider center
        Vector2 raycastOrigin = (Vector2)transform.position;
        RaycastHit2D leftHit = Physics2D.Raycast(raycastOrigin, Vector2.left, wallCheckDistance, wallLayer);
        isWallLeft = leftHit.collider != null;

        // Raycast right from collider center
        RaycastHit2D rightHit = Physics2D.Raycast(raycastOrigin, Vector2.right, wallCheckDistance, wallLayer);
        isWallRight = rightHit.collider != null;
    }

    public bool IsGrounded => isGrounded;
    public bool WasGrounded => wasGrounded;
    public bool JustGrounded => isGrounded && !wasGrounded;
    public bool JustLeftGround => !isGrounded && wasGrounded;
    public bool IsWallLeft => isWallLeft;
    public bool IsWallRight => isWallRight;
    public bool IsTouchingWall => isWallLeft || isWallRight;

    private void OnDrawGizmos()
    {
        if (capsuleCollider == null) return;
        
        // Visualize ground check raycast
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector2 raycastOrigin = (Vector2)transform.position + Vector2.down * (capsuleCollider.size.y / 2);
        Gizmos.DrawLine(raycastOrigin, raycastOrigin + Vector2.down * groundCheckDistance);
        Gizmos.DrawWireSphere(raycastOrigin + Vector2.down * groundCheckDistance, 0.05f);

        // Visualize wall check raycasts
        Vector2 wallRaycastOrigin = (Vector2)transform.position;
        
        // Left wall check
        Gizmos.color = isWallLeft ? Color.green : Color.red;
        Gizmos.DrawLine(wallRaycastOrigin, wallRaycastOrigin + Vector2.left * wallCheckDistance);
        Gizmos.DrawWireSphere(wallRaycastOrigin + Vector2.left * wallCheckDistance, 0.05f);
        
        // Right wall check
        Gizmos.color = isWallRight ? Color.green : Color.red;
        Gizmos.DrawLine(wallRaycastOrigin, wallRaycastOrigin + Vector2.right * wallCheckDistance);
        Gizmos.DrawWireSphere(wallRaycastOrigin + Vector2.right * wallCheckDistance, 0.05f);
    }
}
