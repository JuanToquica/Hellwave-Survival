using System.Security;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform head;
    [SerializeField] private Transform arms;
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    [Header ("Movement")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float flightTime;
    [SerializeField] private float flightVelocity;
    [SerializeField] private float armRotationOffset;
    [SerializeField] private float maxHeadRotation;
    public float raycastDistance;
    private float flightTimer;           
    private float moveInput;
    private float movement;
    private float movementRef;

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
        rb.linearVelocity = new Vector2(movementSpeed * movement, rb.linearVelocity.y);
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
        Vector2 armDirection = mousePos -arms.position;
        Vector2 headDirection;
        float armOffset = armRotationOffset;
        if (direction.normalized.x >= 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            headDirection = mousePos - head.position;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            headDirection = head.position - mousePos;
            armOffset = 90 + (90 - armRotationOffset);
        }

        float armAngle = Mathf.Atan2(armDirection.y, armDirection.x) * Mathf.Rad2Deg;
        float headAngle = Mathf.Atan2(headDirection.y, headDirection.x) * Mathf.Rad2Deg;

        arms.rotation = Quaternion.Euler(0, 0, armAngle + armOffset);
        if (Mathf.Abs(headAngle) < maxHeadRotation)
            head.rotation = Quaternion.Euler(0, 0, headAngle);
        else
            head.rotation = Quaternion.Euler(0, 0, Mathf.Sign(headAngle) * maxHeadRotation);        
    }

    private void SetIsGrounded()
    {
        isGrounded = Physics2D.Raycast(transform.position, -transform.up, raycastDistance, 1 << 3);
        Debug.DrawRay(transform.position, -transform.up * raycastDistance, Color.red);
        if (isGrounded && !flying) canFly = true;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded && ctx.started)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        if (!isGrounded && (ctx.performed || ctx.started) && canFly)
        {
            flying = true;
            canFly = false;
            flightTimer = 0;
        }
        if (ctx.canceled)
        {
            flying = false;
        }
    }


    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            moveInput = ctx.ReadValue<float>();
            animator.SetBool("walking", true);
        }
        if (ctx.canceled)
        {
            animator.SetBool("walking", false);
            moveInput = 0;
        }
    }

}


