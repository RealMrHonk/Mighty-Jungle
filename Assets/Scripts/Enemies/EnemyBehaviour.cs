using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyBehaviorState
{
    Wandering,
    Chasing,
    Attacking
}


public abstract class EnemyBehaviour : MonoBehaviour
{
    public EnemyBehaviorState state { get; private set; } = EnemyBehaviorState.Wandering;

    [SerializeField] protected LayerMask walkableLayer;
    [SerializeField] protected LayerMask attackLayer;

    [SerializeField] protected float detectionRadius;
    [SerializeField] protected float attackRadius;

    private void FixedUpdate()
    {
        
        Collider2D[] detected = Physics2D.OverlapCircleAll((Vector2)transform.position, detectionRadius, attackLayer);
        Collider2D[] attackable = Physics2D.OverlapCircleAll((Vector2)transform.position, attackRadius, attackLayer);

        if (attackable.Length > 0)
        {
            state = EnemyBehaviorState.Attacking;
            Attack(attackable);
        } 
        else if (detected.Length > 0)
        {
            state = EnemyBehaviorState.Chasing;
            Chase(detected);
        }
        else
        {
            state = EnemyBehaviorState.Wandering;
            Wander();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    protected abstract void Wander();
    protected abstract void Chase(Collider2D[] detected);
    protected abstract void Attack(Collider2D[] attackable);
    
}
