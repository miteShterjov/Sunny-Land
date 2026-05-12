using System.Collections;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Enemy : MonoBehaviour
    {
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
        [SerializeField] protected bool isTurnedRifht;
        [SerializeField] protected int facingDir;

        protected float currentHealth;
        protected Animator animator;
        protected Rigidbody2D rb;
        protected Vector3[] patrolPositions;
        protected int currentPatrolIndex;
        protected Vector3 startingPosition;
        protected bool isWaitingAtPatrolPoint;
        protected bool isDead;

        private static readonly int MoveAnimParam = Animator.StringToHash("xVelocity");

        protected virtual void Awake()
        {
            currentHealth = maxHealth;
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            startingPosition = transform.position;
            HandlePatrolPointsToVector3();
            
            if (canPatrol) HandleStateMachine(EnemyState.Patrol);
            else HandleStateMachine(EnemyState.Idle);

            facingDir = isTurnedRifht ? 1 : -1;
        }

        protected virtual void Update()
        {
            HandleCollision();
            HandleDetectionAndStateTransitions();
            HandleCurrentState();
            HandleAnimEvents();
        }

        // handles the state machine transitions and ensures that the same state is not re-entered
        protected virtual void HandleStateMachine(EnemyState newState)
        {
            if (currentState == newState) return;
            currentState = newState; 
            SwichState(currentState);
        }

        // runs every frame and handles the state machine logic for the current state
        protected virtual void HandleCurrentState() => SwichState(currentState);

        protected virtual void SwichState(EnemyState currentState)
        {
            switch (currentState)
            {
                case EnemyState.Idle: HandleIdleState(); break;
                case EnemyState.Patrol: HandlePatrolState(); break;
                case EnemyState.Chase: HandleChaseState(); break;
                case EnemyState.Attack: HandleAttackState(); break;
                case EnemyState.Dead: HandleDeadState(); break;
            }
        }

        protected virtual void HandleDetectionAndStateTransitions()
        {
            if (isDead) 
            {
                HandleStateMachine(EnemyState.Dead);
                return;
            }

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

            if (canPatrol)
            {
                HandleStateMachine(EnemyState.Patrol);
                return;
            }

            HandleStateMachine(EnemyState.Idle);
        }

        protected virtual void HandleIdleState()
        {
            rb.linearVelocity = Vector2.zero;
            if (isWaitingAtPatrolPoint) return;

            float distanceFromStartPos = 0.5f;

            if (Vector2.Distance(transform.position, startingPosition) > distanceFromStartPos)
            {
                float direction = Mathf.Sign(startingPosition.x - transform.position.x);
                rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
                if (direction != facingDir) Flip();
            }

            if (Vector2.Distance(transform.position, startingPosition) <= distanceFromStartPos)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        protected virtual void HandlePatrolState()
        {
            if (patrolPositions.Length == 0) return;
            Vector2 targetPos = patrolPositions[currentPatrolIndex];

            rb.linearVelocity = new Vector2(Mathf.Sign(targetPos.x - transform.position.x) * patrolSpeed, rb.linearVelocity.y);

            if (!isWaitingAtPatrolPoint && Vector2.Distance(transform.position, targetPos) < 0.25f)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPositions.Length;
                StartCoroutine(WaitAtPatrolPointCo());
            }
        }

        protected virtual void HandleChaseState() 
        {
                if (!isPlayerInAggroRange)
                {
                    HandleStateMachine(EnemyState.Idle);
                    return;
                }
    
                rb.linearVelocity = new Vector2(facingDir * chaseSpeed, rb.linearVelocity.y);
    
                if (isPlayerInAttackRange)
                {
                    HandleStateMachine(EnemyState.Attack);
                }
        }

        protected virtual void HandleAttackState() { }
        protected virtual void HandleDeadState() { }

        protected virtual void HandlePatrolPointsToVector3()
        {
            if (patrolPoints.Length == 0) return;
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
            isPlayerInAggroRange = Physics2D.Raycast(transform.position, Vector2.right * facingDir, aggroRange, whatIsPlayer);
            isPlayerInAttackRange = Physics2D.Raycast(transform.position, Vector2.right * facingDir, attackRange, whatIsPlayer);
        }

        protected virtual void OnDrawGizmos()
        {
            // Ground Check
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(
                transform.position,
                new Vector2(transform.position.x, transform.position.y - groundCheckDistance)
                );
            Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y - groundCheckDistance), 0.2f);

            // Wall Check
            Gizmos.color = isWallDetected ? Color.green : Color.red;
            Gizmos.DrawLine(
                transform.position,
                new Vector2(transform.position.x + 0.15f + (wallCheckDistance * facingDir), transform.position.y)
                );
            Gizmos.DrawWireSphere(new Vector2(transform.position.x + 0.15f + (wallCheckDistance * facingDir), transform.position.y), 0.2f);

            // Aggro Check
            Gizmos.color = isPlayerInAggroRange ? Color.red : Color.white;
            Gizmos.DrawLine(
                transform.position,
                new Vector2(transform.position.x - 0.15f + (aggroRange * facingDir), transform.position.y)
                );
            Gizmos.DrawWireSphere(new Vector2(transform.position.x - 0.15f + (aggroRange * facingDir), transform.position.y), 0.2f);

            // Attack Check
            Gizmos.color = isPlayerInAttackRange ? Color.red : Color.white;
            Gizmos.DrawLine(
                transform.position,
                new Vector2(transform.position.x + (attackRange * facingDir), transform.position.y)
                );
            Gizmos.DrawWireSphere(new Vector2(transform.position.x + (attackRange * facingDir), transform.position.y), 0.2f);
        }

        protected IEnumerator WaitAtPatrolPointCo()
        {
            isWaitingAtPatrolPoint = true;
            HandleStateMachine(EnemyState.Idle);
            yield return new WaitForSeconds(1f);
            isWaitingAtPatrolPoint = false;
            Flip();
            HandleStateMachine(EnemyState.Patrol);
        }

        public virtual void TakeDamage(float damage)
        {
            if (isDead) return;
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                isDead = true;
                HandleStateMachine(EnemyState.Dead);
            }
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
