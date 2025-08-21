using System.Security;
using UnityEngine;
using UnityEngine.InputSystem;


public enum PlayerState
{
    Idle = 0,
    Running = 1,
    Jumping = 2,
    Flying = 3,
    Falling = 4
}

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform head;
    [SerializeField] private Transform rightArm;
    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform rightArmTarget;
    [SerializeField] private Transform leftArmTarget;
    [SerializeField] private Transform weaponPivot;
    private Transform rightGripPoint;
    private Transform leftGripPoint;
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    public PlayerState currentState;

    [Header ("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float flightTime;
    [SerializeField] private float flightVelocity;
    [SerializeField] private float armRotationOffset;
    [SerializeField] private float maxHeadRotation;
    [SerializeField] private float maxArmsRotation;
    public float raycastDistance;
    private float flightTimer;           
    private float moveInput;
    private float movement;
    private float movementRef;
    private bool activatedAiming;

    [Header("Debugging")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool flying;
    [SerializeField] private bool canFly;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        flightTimer = 0;
    }

    private void Update()
    {
        movement = Mathf.Clamp(Mathf.SmoothDamp(movement, moveInput, ref movementRef, acceleration), -1, 1);
        SetIsGrounded();
        SetCurrentState();
        if (flying) SetFlightTimer();
    }

    private void FixedUpdate()
    {
        if (flying) Fly();
        ApplyMovement();
        Aim();
    }

    private void ApplyMovement()
    {
         rb.linearVelocity = new Vector2(speed * movement, rb.linearVelocity.y);        
    }

    private void SetFlightTimer()
    {
        if (flightTimer < flightTime)
        {
            flightTimer += Time.deltaTime;
            if (flightTimer > flightTime)
            {
                flying = false;
                flightTimer = 0;
            }
        }
    }

    private void Fly()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, flightVelocity);
    }
    private void Aim()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        Vector2 weaponDirection;
        Vector2 headDirection;
        float armOffset = armRotationOffset;
        if (direction.normalized.x >= 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            headDirection = mousePos - head.position;
            weaponDirection = mousePos - weaponPivot.position;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            headDirection = head.position - mousePos;
            weaponDirection = weaponPivot.position - mousePos;
            armOffset = 90 + (90 - armRotationOffset);
        }
  
        float headAngle = Mathf.Atan2(headDirection.y, headDirection.x) * Mathf.Rad2Deg;
        if (Mathf.Abs(headAngle) < maxHeadRotation)
            head.rotation = Quaternion.Euler(0, 0, headAngle);
        else
            head.rotation = Quaternion.Euler(0, 0, Mathf.Sign(headAngle) * maxHeadRotation);

        if (activatedAiming)
        {
            float armAngle = Mathf.Atan2(weaponDirection.y, weaponDirection.x) * Mathf.Rad2Deg;
            if (Mathf.Abs(armAngle) < maxArmsRotation)
                weaponPivot.rotation = Quaternion.Euler(0, 0, armAngle);
            else
                weaponPivot.rotation = Quaternion.Euler(0, 0, Mathf.Sign(armAngle) * maxArmsRotation);
        }
        else
        {
            weaponPivot.rotation = Quaternion.identity;
        }

        rightArmTarget.position = rightGripPoint.position;
        leftArmTarget.position = leftGripPoint.position;
    }
    private void SetIsGrounded()
    {
        isGrounded = Physics2D.Raycast(transform.position + transform.right * (0.75f / 2), -transform.up, raycastDistance, 1 << 3) 
            || Physics2D.Raycast(transform.position - transform.right * (0.75f / 2), -transform.up, raycastDistance, 1 << 3);
        Debug.DrawRay(transform.position + transform.right * (0.75f / 2), -transform.up * raycastDistance, Color.red);
        Debug.DrawRay(transform.position - transform.right * (0.75f / 2), -transform.up * raycastDistance, Color.red);

        if (isGrounded && !flying && !canFly) canFly = true;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded && ctx.started)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        if (!isGrounded && (ctx.performed || ctx.started) && canFly)
        {
            flying = true;
            canFly = false;
            flightTimer = 0;
        }
        if (ctx.canceled)
            flying = false;
    }


    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            moveInput = ctx.ReadValue<float>();                
        if (ctx.canceled)
            moveInput = 0;
    }

    public void ChangeGripPoints(Transform newRightGripPoint, Transform newLeftGripPoint, bool useLeftArmBackPosition, bool requiresAiming)
    {
        rightGripPoint = newRightGripPoint;
        leftGripPoint = newLeftGripPoint;

        if (useLeftArmBackPosition)
            leftArm.localPosition = rightArm.localPosition;
        else
            leftArm.localPosition = new Vector3(0, leftArm.localPosition.y, leftArm.localPosition.z);

        activatedAiming = requiresAiming;
    }

    private void SetCurrentState()
    {
        if (isGrounded && moveInput == 0)
            currentState = PlayerState.Idle;
        else if (isGrounded && moveInput != 0)
        {
            currentState = PlayerState.Running;
            if (moveInput > 0 && transform.localScale.x == 1 || moveInput < 0 && transform.localScale.x == -1)
                animator.SetFloat("Direction", 1);
            else
                animator.SetFloat("Direction", -1);
        }
        else if (flying == true)
        {
            currentState = PlayerState.Flying;
        }
        else if (rb.linearVelocity.y > 0.1f)
        {
            currentState = PlayerState.Jumping;
        }
        else
        {
            currentState = PlayerState.Falling;
        }
        animator.SetInteger("State", (int)currentState);
    }

}


