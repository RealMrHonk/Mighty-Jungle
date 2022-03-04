using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Basic2DPlatformerController controller;
    [SerializeField] private MeleeAttack attackScript;
    [SerializeField] private Health health;

    [Header("Idle")] [SerializeField] private string AnimPlayerIdle = "Player_Idle";

    [Header("Move")]
    [SerializeField] private string animPlayerRun = "Player_Run";
    [SerializeField] private float minRunSpeed = 0.1f;

    [Header("Jump")]
    [SerializeField] private string animPlayerJump = "Player_Jump";
    [SerializeField] private string animPlayerFall = "Player_Fall";

    [Header("Attack")]
    [SerializeField] private string[] animPlayerAttack = new string[] { "Player_Attack1", "Player_Attack2", "Player_Attack3" };
    [SerializeField] private float attackChainThreshold = 0.1f;
    private bool isAttacking = false;
    private int attackChainIndex = 0;
    private float nextPossibleAttackTime = float.MinValue;

    [Header("Hurt")] 
    [SerializeField] private string animPlayerHurt = "";

    [Header("Die")]
    [SerializeField] private string animPlayerDie = "";

    private void Start()
    {
        attackScript.OnAttack += StartAttack;
    }

    // Update is called once per frame
    private void Update()
    {

        controller.inputEnabled = !isAttacking;

        // Facing left or right
        if (controller.velocity.x > minRunSpeed)
        {
            transform.localScale = Vector3.one;
        }
        else if (controller.velocity.x < -minRunSpeed) 
        { 
            transform.localScale = new Vector3(-1, 1, 1);
        }


        if (controller.isGrounded)
        {
            if (isAttacking)
            {
                animator.Play(animPlayerAttack[attackChainIndex]);
            }
            else                                                        // Dont do move animations when attacking
            {
                // Moving
                if (Mathf.Abs(controller.velocity.x) > minRunSpeed)
                {
                    animator.Play(animPlayerRun);
                }
                else
                {
                    animator.Play(AnimPlayerIdle);
                }
            }

            
        }
        else
        {
            if (controller.isFalling)
            {
                animator.Play(animPlayerFall);
            }
            else if(controller.jumping)
            {
                animator.Play(animPlayerJump);
            }
        }
    }
    
    private void StartAttack()
    {
        if (isAttacking) return;

        if (nextPossibleAttackTime + attackChainThreshold < Time.time)
        {
            attackChainIndex = 0;
        }
        else
        {
            attackChainIndex++;
            attackChainIndex = RoundRobinAnimIndex(attackChainIndex, 0, animPlayerAttack.Length - 1);
        }
        isAttacking = true;   
    }

    public void AttackFinished()
    {
        isAttacking = false;
        nextPossibleAttackTime = Time.time;
    }
    
    private int RoundRobinAnimIndex(int index, int min, int max)
    {
        if (index > max) return min;
        return index;
    }
}
