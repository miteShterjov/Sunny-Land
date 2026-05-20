using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Enemy : MonoBehaviour
    {
        public bool IsKnockback { get => isKnockback; set => isKnockback = value; }

        [Header("Stats")]
        [SerializeField] protected float maxHealth;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float patrolSpeed;
        [SerializeField] protected float chaseSpeed;
        [SerializeField] protected EnemyState currentState;
        [Header("Detection")]
        [SerializeField] protected float aggroRange;
        [SerializeField] protected float attackRange;
        [Header("References")]
        [SerializeField] protected bool canPatrol;
        [SerializeField] protected Transform[] patrolPoints;
        [Header("Collision Checks")]
        [SerializeField] protected float groundCheckDistance;
        [SerializeField] protected float wallCheckDistance;
        [SerializeField] protected bool isGrounded;
        [SerializeField] protected bool isWallDetected;
        [SerializeField] protected bool isPlayerInAggroRange;
        [SerializeField] protected bool isPlayerInAttackRange;
        [SerializeField] protected LayerMask whatIsGround;
        [SerializeField] protected LayerMask whatIsPlayer;
        [FormerlySerializedAs("isTurnedRifht")]
        [SerializeField] protected bool isTurnedRight;
        [SerializeField] protected int facingDir;
        [FormerlySerializedAs("isKnockbacked")]
        [SerializeField] private bool isKnockback;

        protected Rigidbody2D rb;
        protected Transform playerTransform;
        protected Animator animator;

        private float currentHealth;
        private bool isDead;
        private Vector3[] patrolPositions;
        private int currentPatrolIndex;
        private Vector3 startingPosition;
        private bool isWaitingAtPatrolPoint;

        private static readonly int MoveAnimParam = Animator.StringToHash("xVelocity");

        protected virtual void Awake()
        {
            currentHealth = maxHealth;
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody2D>();
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        protected virtual void Start()
        {
            startingPosition = transform.position;
            HandlePatrolPointsToVector3();
            facingDir = isTurnedRight ? 1 : -1;
            currentState = canPatrol ? EnemyState.Patrol : EnemyState.Idle;
        }

        protected virtual void Update()
        {
            HandleCollision();
            HandleDetectionAndStateTransitions();
            HandleCurrentState();
            HandleAnimEvents();
        }

        // Only sets state — does NOT run state logic
        protected virtual void HandleStateMachine(EnemyState newState) => currentState = newState;

        // Runs state logic every frame
        protected virtual void HandleCurrentState()
        {
            switch (currentState)
            {
                case EnemyState.Idle:   HandleIdleState();   break;
                case EnemyState.Patrol: HandlePatrolState(); break;
                case EnemyState.Chase:  HandleChaseState();  break;
                case EnemyState.Attack: HandleAttackState(); break;
                case EnemyState.Dead:   HandleDeadState();   break;
            }
        }

        protected virtual void HandleDetectionAndStateTransitions()
        {
            if (isDead)
            {
                HandleStateMachine(EnemyState.Dead);
                return;
            }

            // Don't interrupt a timed patrol wait
            if (isWaitingAtPatrolPoint) return;

            if (isPlayerInAttackRange)
            {
                HandleStateMachine(EnemyState.Attack);
                return;
            }

            if (isPlayerInAggroRange)
            {
                HandleStateMachine(EnemyState.Chase);
                return;
            }

            HandleStateMachine(canPatrol ? EnemyState.Patrol : EnemyState.Idle);
        }

        protected virtual void HandleIdleState()
        {
            if (isWaitingAtPatrolPoint)
            {
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
                return;
            }

            float distanceFromStart = Vector2.Distance(transform.position, startingPosition);

            if (distanceFromStart > 0.5f)
            {
                float direction = Mathf.Sign(startingPosition.x - transform.position.x);
                if (!Mathf.Approximately(direction, facingDir)) Flip();
                rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            }
        }

        protected virtual void HandlePatrolState()
        {
            if (patrolPositions == null || patrolPositions.Length == 0) return;

            Vector2 targetPos = patrolPositions[currentPatrolIndex];
            int dirToTarget = (int)Mathf.Sign(targetPos.x - transform.position.x);

            // Flip to face patrol direction
            if (dirToTarget != 0 && dirToTarget != facingDir) Flip();

            rb.linearVelocity = new Vector2(dirToTarget * patrolSpeed, rb.linearVelocity.y);

            if (isWaitingAtPatrolPoint || !(Vector2.Distance(transform.position, targetPos) < 0.25f)) return;
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPositions.Length;
            StartCoroutine(WaitAtPatrolPointCo());
        }

        protected virtual void HandleChaseState()
        {
            if (!playerTransform) return;

            int dirToPlayer = playerTransform.position.x > transform.position.x ? 1 : -1;
            if (dirToPlayer != facingDir) Flip();

            rb.linearVelocity = new Vector2(facingDir * chaseSpeed, rb.linearVelocity.y);
        }

        protected virtual void HandleAttackState() { }
        protected virtual void HandleDeadState() { }

        protected virtual void HandlePatrolPointsToVector3()
        {
            if (patrolPoints == null || patrolPoints.Length == 0) return;
            patrolPositions = new Vector3[patrolPoints.Length];
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                patrolPositions[i] = patrolPoints[i].position;
                Destroy(patrolPoints[i].gameObject);
            }
        }

        protected virtual void Flip()
        {
            facingDir *= -1;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        protected virtual void HandleAnimEvents()
        {
            animator.SetFloat(MoveAnimParam, rb.linearVelocity.x);
        }

        protected virtual void HandleCollision()
        {
            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
            isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

            // Cast both directions so player is detected regardless of which side they're on
            isPlayerInAggroRange =
                Physics2D.Raycast(transform.position, Vector2.right, aggroRange, whatIsPlayer) ||
                Physics2D.Raycast(transform.position, Vector2.left,  aggroRange, whatIsPlayer);

            isPlayerInAttackRange =
                Physics2D.Raycast(transform.position, Vector2.right, attackRange, whatIsPlayer) ||
                Physics2D.Raycast(transform.position, Vector2.left,  attackRange, whatIsPlayer);
        }

        protected virtual void OnDrawGizmos()
        {
            // Ground check
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position,
                new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
            Gizmos.DrawWireSphere(
                new Vector2(transform.position.x, transform.position.y - groundCheckDistance), 0.2f);

            // Wall check
            Gizmos.color = isWallDetected ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position,
                new Vector2(transform.position.x + 0.15f + wallCheckDistance * facingDir, transform.position.y));
            Gizmos.DrawWireSphere(
                new Vector2(transform.position.x + 0.15f + wallCheckDistance * facingDir, transform.position.y), 0.2f);

            // Aggro range — sphere reflects bidirectional detection
            Gizmos.color = isPlayerInAggroRange ? Color.red : Color.white;
            Gizmos.DrawWireSphere(transform.position, aggroRange);

            // Attack range — sphere reflects bidirectional detection
            Gizmos.color = isPlayerInAttackRange ? Color.red : Color.white;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        private IEnumerator WaitAtPatrolPointCo()
        {
            isWaitingAtPatrolPoint = true;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            HandleStateMachine(EnemyState.Idle);
            yield return new WaitForSeconds(1f);
            isWaitingAtPatrolPoint = false;
            HandleStateMachine(EnemyState.Patrol);
            Flip();
        }

        public virtual void TakeDamage(float damage)
        {
            if (isDead) return;
            currentHealth -= damage;
            if (!(currentHealth <= 0)) return;
            isDead = true;
            HandleStateMachine(EnemyState.Dead);
        }

        public virtual void TakeHeal(float healAmount)
        {
            if (isDead) return;
            currentHealth += healAmount;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
        }
    }

    public enum EnemyState { Idle, Patrol, Chase, Attack, Dead }
}