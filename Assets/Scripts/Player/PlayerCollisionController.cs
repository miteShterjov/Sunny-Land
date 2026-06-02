using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class PlayerCollisionController : MonoBehaviour
    {
        [Header("Ground Detection")]
        [SerializeField] private float groundCheckDistance = 0.1f;
        [SerializeField] private LayerMask groundLayer;
        [Header("Wall Detection")]
        [SerializeField] private float wallCheckDistance = 0.1f;
        [SerializeField] private LayerMask wallLayer;
        [Header("Collision States")]
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isWallLeft;
        [SerializeField] private bool isWallRight;
        [SerializeField] private bool isOnStairs;
    
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
        
        public bool IsOnStairs { get => isOnStairs; private set => isOnStairs = value;}
        public bool IsGrounded => isGrounded;
        public bool IsWallLeft => isWallLeft;
        public bool IsWallRight => isWallRight;
        public bool IsTouchingWall => isWallLeft || isWallRight;

        private void UpdateGroundDetection()
        {
            // Raycast down from collider bottom
            Vector2 raycastOrigin = (Vector2)transform.position + Vector2.down * (capsuleCollider.size.y / 2);
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, groundCheckDistance, groundLayer);

            isGrounded = hit.collider;
        }

        private void UpdateWallDetection()
        {
            // Raycast left from collider center
            Vector2 raycastOrigin = transform.position;
            RaycastHit2D leftHit = Physics2D.Raycast(raycastOrigin, Vector2.left, wallCheckDistance, wallLayer);
            isWallLeft = leftHit.collider;

            // Raycast right from collider center
            RaycastHit2D rightHit = Physics2D.Raycast(raycastOrigin, Vector2.right, wallCheckDistance, wallLayer);
            isWallRight = rightHit.collider;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Stairs")) IsOnStairs = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Stairs")) IsOnStairs = false;
        }

        private void OnDrawGizmos()
        {
            if (!capsuleCollider) return;
        
            // Visualize ground check raycast
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Vector2 raycastOrigin = (Vector2)transform.position + Vector2.down * (capsuleCollider.size.y / 2);
            Gizmos.DrawLine(raycastOrigin, raycastOrigin + Vector2.down * groundCheckDistance);
            Gizmos.DrawWireSphere(raycastOrigin + Vector2.down * groundCheckDistance, 0.05f);

            // Visualize wall check raycasts
            Vector2 wallRaycastOrigin = transform.position;
        
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
}
