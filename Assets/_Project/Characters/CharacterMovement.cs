using System;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Walking")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float baseDrag;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform feetPoint;
    [SerializeField] private Vector2 overlapBoxSize;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float earlyJumpToleranceDelay;
    [SerializeField] private float lateJumpToleranceDelay;

    [Header("Airborne")]
    [SerializeField] private float fallingForce;
    [SerializeField] private float airDrag;

    [Header("Wall Sliding")]
    [SerializeField] private Vector2 slidingBoxOffset;
    [SerializeField] private Vector2 slidingBoxSize;
    [SerializeField] private LayerMask slidableMask;
    [SerializeField] private float slidingForce;

    private Rigidbody2D rb;
    private int maxJumps = 2;
    private int jumpsLeft;
    private bool isGrounded;
    private float lastGroundedTime;
    private bool isWallSliding;
    private float previousGravityScale;
    private float lastJumpIntentTime = float.MinValue;

    public float WalkInput { get; set; }

    public event Action OnGroundTouched = delegate { };


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void AddMaxJumps(int amount) => maxJumps += amount;
    public void AddJumpForce(float amount) => jumpForce += amount;

    public void Jump(bool automaticEnhancements = false)
    {
        if (automaticEnhancements)
            lastJumpIntentTime = Time.time;
        if (jumpsLeft == 0)
            return;

        jumpsLeft--;
        rb.StopVertically();
        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        CheckIfIsGrounded();
        TryResetAvailableJumps();
        CheckForWallSliding();
        ApplyFallingForce();
        TryInvokeTooEarlyJump();

        rb.AddForceTimesDrag(walkSpeed * WalkInput * Vector2.right);
        WalkInput = 0f;
    }

    private void TryInvokeTooEarlyJump()
    {
        if (Time.time < lastJumpIntentTime + earlyJumpToleranceDelay && jumpsLeft == maxJumps)
        {
            Jump();
            lastJumpIntentTime = float.MinValue;
        }
    }

    private void CheckIfIsGrounded()
    {
        isGrounded = rb.velocity.y <= 0f && Physics2D.OverlapBox(feetPoint.position, overlapBoxSize, 0f, groundMask);
        if (isGrounded)
            lastGroundedTime = Time.time;
    }

    private void TryResetAvailableJumps()
    {
        if (isGrounded)
        {
            jumpsLeft = maxJumps;
            rb.drag = baseDrag;
            OnGroundTouched.Invoke();
        }
        else
        {
            rb.drag = airDrag;
            if (jumpsLeft == maxJumps && Time.time > lastGroundedTime + lateJumpToleranceDelay)
                jumpsLeft--;
        }

    }

    private void ApplyFallingForce()
    {
        if (isWallSliding)
            rb.AddForce(slidingForce * Vector2.down);
        else if (rb.IsFalling())
            rb.AddForce(fallingForce * rb.gravityScale * Vector2.down);
    }

    private void CheckForWallSliding()
    {
        if (WalkInput == 0f || isGrounded)
        {
            SetSliding(false);
            return;
        }

        Vector3 slidingBoxPosition = transform.position + (Vector3)slidingBoxOffset;

        if (WalkInput > 0f)
        {
            SetSliding(GrabCheck(slidingBoxPosition));
        }
        else
        {
            slidingBoxPosition.x -= 2f * slidingBoxOffset.x;
            SetSliding(GrabCheck(slidingBoxPosition));
        }
    }

    private bool GrabCheck(Vector3 position)
    {
        return rb.velocity.y <= 0f && Physics2D.OverlapBox(position, slidingBoxSize, 0f, slidableMask);
    }

    private void SetSliding(bool enable)
    {
        isWallSliding = enable;
        if (rb.gravityScale > 0f)
            previousGravityScale = rb.gravityScale;

        rb.gravityScale = enable ? 0f : previousGravityScale;
    }

    private void OnDrawGizmosSelected()
    {
        if (feetPoint)
            Gizmos.DrawCube(feetPoint.position, overlapBoxSize);

        Vector3 slidingOffset = slidingBoxOffset;
        Gizmos.DrawCube(transform.position + slidingOffset, slidingBoxSize);
        slidingOffset.x -= 2f * slidingBoxOffset.x;
        Gizmos.DrawCube(transform.position + slidingOffset, slidingBoxSize);
    }
}
