using System.Security;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;


public enum PlayerState
{
    Idle = 0,
    Running = 1,
    Jumping = 2,
    Flying = 3,
    Falling = 4,
    TakingDamage = 5
        
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
    [SerializeField] private float currentAimingOffset;
    [SerializeField] private float maxHeadRotation;
    [SerializeField] private float maxArmsRotation;
    [SerializeField] private float knockbackForce;    
    public float raycastDistance;
    private float flightTimer;           
    private float moveInput;
    private float movement;
    private float movementRef;
    private bool activatedAiming;
    public bool takingDamage;

    [Header("Debugging")]   
    [SerializeField] private bool flying;
    [SerializeField] private bool canFly;
    public bool isGrounded;

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
        if (!takingDamage)
            Aim();            
    }

    public Vector2 GetLinearVelocity()
    {
        return rb.linearVelocity;
    }
    private void ApplyMovement()
    {
        Vector2 desiredVelocity = new Vector2(speed * moveInput ,rb.linearVelocity.y);
        float smoothFactor = takingDamage ? 0.1f : acceleration;
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, desiredVelocity, smoothFactor);
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
        float armOffset = currentAimingOffset;
        if (direction.normalized.x >= 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            headDirection = mousePos - head.position;
            weaponDirection = mousePos - (weaponPivot.position + weaponPivot.up * currentAimingOffset);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            headDirection = head.position - mousePos;
            weaponDirection = (weaponPivot.position + weaponPivot.up * currentAimingOffset) - mousePos;
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
                weaponPivot.rotation = Quaternion.Euler(0, 0, armAngle + armOffset);
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
        int layer = 1 << 3 | 1 << 8;
        isGrounded = Physics2D.Raycast(transform.position + transform.right * ((0.73f / 2) - 0.03f), -transform.up, raycastDistance, layer) //0.03 is the offset of the collider
            || Physics2D.Raycast(transform.position - transform.right * ((0.73f / 2) + 0.03f), -transform.up, raycastDistance, layer);
        Debug.DrawRay(transform.position + transform.right * ((0.73f / 2) - 0.03f), -transform.up * raycastDistance, Color.red);
        Debug.DrawRay(transform.position - transform.right * ((0.73f / 2) + 0.03f), -transform.up * raycastDistance, Color.red);

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

    public void ChangeGripPoints(Transform newRightGripPoint, Transform newLeftGripPoint, bool useLeftArmBackPosition, bool requiresAiming, float aimingOffset)
    {
        rightGripPoint = newRightGripPoint;
        leftGripPoint = newLeftGripPoint;

        if (useLeftArmBackPosition)
            leftArm.localPosition = rightArm.localPosition;
        else
            leftArm.localPosition = new Vector3(0, leftArm.localPosition.y, leftArm.localPosition.z);

        activatedAiming = requiresAiming;
        currentAimingOffset = aimingOffset;
    }

    private void SetCurrentState()
    {
        if (takingDamage)
            currentState = PlayerState.TakingDamage;
        else if (isGrounded && moveInput == 0)
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

    public void ApplyKnockback(Vector2 direction, float stunDuration)
    {
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        takingDamage = true;
        head.rotation = Quaternion.identity;        
        weaponPivot.rotation = Quaternion.identity;
        rightArmTarget.position = rightGripPoint.position;
        leftArmTarget.position = leftGripPoint.position;
        Invoke("SetTakingDamage", stunDuration);
    }

    private void SetTakingDamage()
    {
        takingDamage = false;
    }

}


