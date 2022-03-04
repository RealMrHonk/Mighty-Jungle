using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : EnemyBehaviour
{
    [SerializeField] private Basic2DPlatformerController controller;
    [SerializeField] private MeleeAttack biteAttack;

    protected override void Attack(Collider2D[] attackable)
    {
        biteAttack.Attack();
    }

    protected override void Chase(Collider2D[] detected)
    {
        GameObject target = detected[0].gameObject;

        float xMovement = target.transform.position.x > transform.position.x ? 1 : -1;

        controller.HandleInputs(new FrameInput { xMovement = xMovement }); 
    }

    protected override void Wander()
    {
        controller.HandleInputs(new FrameInput { xMovement = 0 });
    }
}
