using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Basic2DPlatformerController controller;
    [SerializeField] private Health health;
    [SerializeField] private SlimeBehaviour behaviour;

    [Header("Idle")] [SerializeField] private string animIdle = "Slime_Idle";

    [Header("Move")]
    [SerializeField] private string animMove = "Slime_Move";
    [SerializeField] private float minMoveSpeed = 0.1f;

    [Header("Attack")]
    [SerializeField] private string animAttack = "Slime_Attack";
    [SerializeField] private float attackChainThreshold = 0.1f;
    private bool isAttacking = false;
    private int attackChainIndex = 0;
    private float nextPossibleAttackTime = float.MinValue;

    [Header("Hurt")]
    [SerializeField] private string animHurt = "Slime_Hurt";
    private bool isHurting = false;

    [Header("Die")]
    [SerializeField] private string animDie = "Slime_Die";


    private void Start()
    {
        health.OnDamaged += StartHurting;
    }

    void Update()
    {
        

        // Facing left or right
        if (controller.velocity.x > minMoveSpeed)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (controller.velocity.x < -minMoveSpeed)
        {
            transform.localScale = Vector3.one;
        }


        if (health.isDead)
        {
            animator.Play(animDie);
            controller.inputEnabled = false;
            Destroy(gameObject, 2);
            return;
        }

        if (isHurting)
        {
            animator.Play(animHurt);
            return;
        }

        if (behaviour.state == EnemyBehaviorState.Attacking)
        {
            animator.Play(animAttack);
            controller.inputEnabled = false;
            return;
        }

        // Moving
        if (Mathf.Abs(controller.velocity.x) > minMoveSpeed)
        {
            animator.Play(animMove);
        }
        else
        {
            animator.Play(animIdle);
        }

        controller.inputEnabled = true;
    }

    private void StartHurting()
    {
        animator.Play(animHurt, 0, 0);
        isHurting = true;
        controller.inputEnabled = false;
    }

    private void StopHurting() => isHurting = false;
}
