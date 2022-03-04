using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{

    [SerializeField] private Basic2DPlatformerController controller;
    [SerializeField] private MeleeAttack swordAttack;

    // Update is called once per frame
    void Update()
    {
        controller.HandleInputs(new FrameInput {
            xMovement = Input.GetAxisRaw("Horizontal"),
            JumpDown = Input.GetButtonDown("Jump"),
            JumpUp = Input.GetButtonUp("Jump")
        });

        if (Input.GetButtonDown("Fire1"))
        {
            if (controller.isGrounded)
            {
                swordAttack.Attack();
            }
        }
    }
}
