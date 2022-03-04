using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FrameInput
{
    public float xMovement;
    public bool JumpDown;
    public bool JumpUp;
}

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D))]
public class Basic2DPlatformerController : MonoBehaviour
{
    public bool inputEnabled { get; set; } = true;

    [Header("Ground Check")]
    [SerializeField] private Vector2 groundCheckPoint;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Head Check")]
    [SerializeField] private Vector2 headCheckPoint;
    [SerializeField] private float headCheckRadius;
    [SerializeField] private LayerMask headLayerMask;
    private bool isHeadBumping = false;

    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    public float MovementSpeed{ get { return movementSpeed; } set { movementSpeed = value; } }
    public Vector3 velocity { get; private set; }

    [Header("Gravity")]
    [SerializeField] private float gravity = -30;
    [SerializeField] private float minFallVelocity = -7.5f;
    public bool isFalling => velocity.y < minFallVelocity;

    [Header("Jumping")]
    [SerializeField] private float jumpStrength = 1000;
    public bool isGrounded { get; private set; }
    public bool jumping { get; private set; }
    private bool hasJumpBuffered = false;
    [SerializeField] private float jumpEndedEarlyGravityModifier = 3;
    private bool hasEndedJumpEarly = true;

    private Rigidbody2D rigidbody;
    private Vector3 lastPosition;
    private float currentHorizontalSpeed = 0, currentVerticalSpeed = 0;
    

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        velocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;

         if (!inputEnabled) currentHorizontalSpeed = 0;

        HandleGravity();
        HandleJumping();

        rigidbody.velocity = new Vector2(currentHorizontalSpeed, currentVerticalSpeed) * Time.fixedDeltaTime;
    }

    private void HandleGravity()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position + groundCheckPoint, groundCheckRadius, groundLayerMask);
        isGrounded = colliders.Length > 0;

        if (isGrounded)
        {
            if (currentVerticalSpeed < 0) currentVerticalSpeed = 0;
        }
        else
        {
            float fallSpeed = hasEndedJumpEarly && currentVerticalSpeed > 0 ? gravity * jumpEndedEarlyGravityModifier : gravity;
            currentVerticalSpeed += fallSpeed;
        }
    }

    private void HandleJumping()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position + headCheckPoint, headCheckRadius, headLayerMask);
        isHeadBumping = colliders.Length > 0;

        if (isGrounded)
        {
            if (currentVerticalSpeed <= 0) jumping = false;

            if (hasJumpBuffered)
            {
                jumping = true;
                hasEndedJumpEarly = false;
                currentVerticalSpeed = jumpStrength;
                hasJumpBuffered = false;
            }
        }

        if (isHeadBumping)
        {
            if (currentVerticalSpeed > 0) currentVerticalSpeed = gravity;
        }
        
    }

    public void HandleInputs(FrameInput input)
    {
        if (!inputEnabled) return;

        currentHorizontalSpeed = input.xMovement * movementSpeed;

        //Jump
        if (input.JumpDown)
        {
            if (isGrounded)
            {
                jumping = true; 
                hasEndedJumpEarly = false; 
                currentVerticalSpeed = jumpStrength;
            }
            else
            {
                hasJumpBuffered = true;
            }
        }

        //Early Jump Cancel
        if (input.JumpUp && !isGrounded && !hasEndedJumpEarly)
        {
            hasEndedJumpEarly = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckPoint, groundCheckRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + headCheckPoint, headCheckRadius);
    }
}
