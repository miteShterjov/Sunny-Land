using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Enemies
{
    public class MushroomEnemy : Enemy
    {
        [Header("Mushroom Settings")]
        [SerializeField] private bool isAttacking;
        [SerializeField] private GameObject gasCloudPrefab;
        [SerializeField] private Transform gasCloudSpawnPoint;
        [SerializeField] private float attackCooldown = 1f;

        private static readonly int AttackParam = Animator.StringToHash("Attack");
        private static readonly int JumpParam = Animator.StringToHash("Jump");

        protected override void HandleDetectionAndStateTransitions()
        {
            if (isAttacking) return;
            base.HandleDetectionAndStateTransitions();
        }

        protected override void HandleAttackState()
        {
            if (!isAttacking)
            {
                DoSpecialAttack();
                isAttacking = true;
            }
        }

        private void DoSpecialAttack()
        {
            animator.SetTrigger(AttackParam);
            GameObject gasCloud = Instantiate(gasCloudPrefab, gasCloudSpawnPoint.transform.position, quaternion.identity);
            StartCoroutine(AttackCooldownCo());
        }

        private IEnumerator AttackCooldownCo()
        {
            yield return new WaitForSeconds(attackCooldown);
            isAttacking = false;
        }
    }
}
