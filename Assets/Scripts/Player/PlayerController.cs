using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private Animator animator;
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
    }

    private void ApplyMovement()
    {
        if (moveInput != 0)
            rb.linearVelocity = new Vector2(movementSpeed * moveInput, rb.linearVelocity.y);
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
            moveInput = ctx.ReadValue<float>();
        if (ctx.canceled)
            moveInput = 0;
    }

    public void OnLook(int layerIndex)
    {
        
    }


}
