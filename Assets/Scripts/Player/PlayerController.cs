using System.Security;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform head;
    [SerializeField] private Transform arms;
    [SerializeField] private float armRotationOffset;
    [SerializeField] private float maxHeadRotation;
    public PlayerInput playerInput;
    public bool isGrounded;
    public float raycastDistance;
    private Rigidbody2D rb;
    private float moveInput;
    private float movement;
    private float movementRef;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movement = Mathf.Clamp(Mathf.SmoothDamp(movement, moveInput, ref movementRef, acceleration), -1, 1);
        SetIsGrounded();

    }

    private void FixedUpdate()
    {
        ApplyMovement();
        Aim();
    }

    private void ApplyMovement()
    {
        if (moveInput != 0)
            rb.linearVelocity = new Vector2(movementSpeed * moveInput, rb.linearVelocity.y);
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
        Debug.DrawRay(transform.position, -transform.up * raycastDistance);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
            rb.AddForce(new Vector2(0, jumpForce));
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


