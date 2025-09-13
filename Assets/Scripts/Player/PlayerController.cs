using System;
using System.Security;
using Unity.VisualScripting;
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
    public static event Action<float, float> OnFlyingTimerChanged;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Animator jetpackAnimator;
    [SerializeField] private Transform head;
    [SerializeField] private Transform rightArm;
    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform rightArmTarget;
    [SerializeField] private Transform leftArmTarget;
    [SerializeField] private Transform weaponPivot;
    public Transform rightGripPoint;
    public Transform leftGripPoint;
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    public PlayerState currentState;

    [Header ("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float flightTime;
    [SerializeField] private float flightVelocity;
    [SerializeField] private float currentAimingOffset;
    [SerializeField] private float maxHeadRotation;
    [SerializeField] private float maxArmsRotation;
    public float knockbackForce;    
    public float raycastDistance;         
    private float moveInput;
    public bool activatedAiming;
    public bool takingDamage;
    public float coyoteTimer;

    [Header("Debugging")]   
    [SerializeField] private bool flying;
    [SerializeField] private bool canFly;
    public bool isGrounded;


    private float _flightTimer;
    public float flightTimer
    {
        get { return _flightTimer; }
        set
        {
            if (_flightTimer != value)
            {
                _flightTimer = value;
                OnFlyingTimerChanged?.Invoke(_flightTimer, flightTime);
            }
        }
    }


    private void OnEnable() => PlayerHealth.OnPlayerDeath += OnDie;
    private void OnDisable() => PlayerHealth.OnPlayerDeath -= OnDie;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        flightTimer = flightTime;
    }

    private void Update()
    {
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
        if (flightTimer > 0)
        {
            flightTimer -= Time.deltaTime;
            if (flightTimer <= 0)
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
        int layer = 1 << 3 | 1 << 8 | 1 << 9 | 1 << 10;
        Vector3 origin = transform.position + transform.right * 0.03f * transform.localScale.x;
        isGrounded = Physics2D.Raycast(origin  + transform.right * (0.73f / 2), -transform.up, raycastDistance, layer) //0.03 is the offset of the collider
            || Physics2D.Raycast(origin - transform.right * (0.73f / 2), -transform.up, raycastDistance, layer);
        Debug.DrawRay(origin + transform.right * (0.73f / 2), -transform.up * raycastDistance, Color.red);
        Debug.DrawRay(origin - transform.right * (0.73f / 2), -transform.up * raycastDistance, Color.red);

        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
            flightTimer = flightTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (isGrounded && !flying && !canFly) canFly = true;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (coyoteTimer > 0 && ctx.started)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            coyoteTimer = 0;
        }        
        else if (!isGrounded && coyoteTimer <= 0 &&(ctx.performed || ctx.started) && canFly)
        {
            flying = true;
            canFly = false;
            flightTimer = flightTime;
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
        else if (isGrounded && moveInput == 0 && Mathf.Abs(rb.linearVelocity.x) < speed - 1.5f)
            currentState = PlayerState.Idle;
        else if (isGrounded && (moveInput != 0 || Mathf.Abs(rb.linearVelocity.x) >= speed - 1.5f))
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
        jetpackAnimator.SetInteger("State", (int)currentState);
    }

    public void ApplyKnockback(Vector2 direction, float stunDuration)
    {
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        takingDamage = true;
        if (direction.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
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

    public void OnDie()
    {
        animator.SetTrigger("Die");
        rb.constraints = RigidbodyConstraints2D.None;
        rb.linearDamping = 1;
        rb.angularDamping = 0;
        rb.AddTorque(5 , ForceMode2D.Impulse);
        head.rotation = Quaternion.identity;
        this.enabled = false;
    }
}


