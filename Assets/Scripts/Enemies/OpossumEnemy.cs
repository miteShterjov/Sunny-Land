using LevelMech;
using Misc;
using Player;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(VFXs))]
    public class OpossumEnemy : Enemy
    {
        [Header("Charge Settings")]
        [SerializeField] private float chargeSpeed;
        [SerializeField] private TrailRenderer chargeTrail;
        [SerializeField] private bool isAttacking;
        [SerializeField] private float damageAmount = 25f;

        private static readonly int EnemyDeathAnimParam = Animator.StringToHash("isDead");

        protected override void HandleDetectionAndStateTransitions()
        {
            if (isAttacking) return;
            base.HandleDetectionAndStateTransitions();
        }

        protected override void HandleAttackState()
        {
            if (!isAttacking)
            {
                DoAttackSequence();
                isAttacking = true;
            }
        }

        private void DoAttackSequence()
        {
            // face player before charging if in range
            if (playerTransform)
            {
                int dirToPlayer = playerTransform.position.x > transform.position.x ? 1 : -1;
                if (dirToPlayer != facingDir) Flip();
            }

            chargeTrail.emitting = true;
            rb.linearVelocity = new Vector2(facingDir * chargeSpeed, rb.linearVelocity.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            PlayerHealthController player = collision.gameObject.GetComponent<PlayerHealthController>();

            rb.linearVelocity = Vector2.zero;
            chargeTrail.emitting = false;

            player.Damage(transform, damageAmount);
            GetComponent<LootDropHandler>().DropLoot();
            animator.SetTrigger(EnemyDeathAnimParam);
        }
    }
}