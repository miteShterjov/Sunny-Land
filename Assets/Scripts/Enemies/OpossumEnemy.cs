using System;
using System.Collections;
using Enemies;
using Misc;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(VFXs))]
public class OpossumEnemy : Enemy
{
    [Header("Opossum Specific")]
    [Header("Charge Settings")]
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeDuration;
    [SerializeField] private TrailRenderer chargeTrail;
    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private bool isKnockbacked;
    [SerializeField] private bool isAttacking;

    private bool playerWasHit;
    private VFXs visuals;

    protected override void Awake()
    {
        base.Awake();
        visuals = GetComponent<VFXs>();
    }

    protected override void HandleAttackState()
    {
        // Opossum specific attack behavior
        // it will do a quick dash towards the player and then be knocked back for a short distance,
        // after the knockback it will be immobile(dazed like) for a short time and then it will be able 
        // to attack again
        if (!isAttacking) StartCoroutine(AttackSequenceCo());
    }

    private IEnumerator AttackSequenceCo()
    {
        isAttacking = true;

        yield return StartCoroutine(ChargeCo());
        yield return StartCoroutine(KnockbackCo());
    
    }

    private IEnumerator ChargeCo()
    {
        float elapsedTime = 0f;
        playerWasHit = false;

        rb.linearVelocity = new Vector2(facingDir * chargeSpeed, rb.linearVelocity.y);
        chargeTrail.emitting = true;

        while(!playerWasHit && elapsedTime < chargeDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rb.linearVelocity = Vector2.zero;
        chargeTrail.emitting = false;
    }

    private IEnumerator KnockbackCo()
    {
        visuals.EnemyKnockback(transform, GameObject.FindGameObjectWithTag("Player").transform, rb);

        // CONTINUE FROM HERE
        // finish knockback and move on to the next sequence 


        yield return new WaitForSeconds(knockbackDuration);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        playerWasHit = true;
    }
}


