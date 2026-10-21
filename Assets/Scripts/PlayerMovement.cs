using UnityEngine;

// Moves the player with a Rigidbody: 
// A/D or Left/Right arrows to ROTATE, W/S or Up/Down to move Forward/Backward, Space to jump (Hold to charge!).
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 7f;      // units per second
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float turnSpeed = 150f;    // degrees per second
    [SerializeField] private float jumpForce = 5f;      // base strength of the jump
    [SerializeField] private float doubleJumpForce = 4f;

    [Header("Charge Jump Settings")]
    [SerializeField] private float maxChargeBonus = 8f;   // Extra force added at full charge
    [SerializeField] private float maxChargeTime = 1.0f;  // Seconds it takes to reach full charge
    private float currentJumpCharge = 0f;

    [Header("Ground Check Settings")]
    [SerializeField] private float groundDistance = 0.5f; // ray length
    [SerializeField] private LayerMask groundMask;        // ground layers

    private Rigidbody rb;               // the physics body we move
    private Vector3 moveDirection;      // worked out from input in Update()
    private bool isGrounded;            // true when standing on the ground
    public float SpeedMultiplier { get; set; } = 1f;
    private bool jumpRequested = false; // set in Update(), used later
    private bool doubleJumpRequested = false; // set in TryDoubleJump()
    private bool hasDoubleJumped = false;     // one per jump
    private PlayerPowerUps powerUps;

    public bool IsGrounded => isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        powerUps = GetComponent<PlayerPowerUps>();
    }

    private void Update()
    {
        // 1. Ground Check
        isGrounded = Physics.Raycast(transform.position + transform.up * groundDistance / 2, -transform.up, groundDistance, groundMask);

        // Back on the ground: the double jump is available again
        if (isGrounded)
        {
            hasDoubleJumped = false;
        }

        // 2. Read Inputs
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right turn input
        float moveZ = Input.GetAxisRaw("Vertical");  // W/S or Up/Down forward/backward

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        // Rotate the player transform in Update for responsive turning visuals
        moveDirection = cameraForward * moveZ + cameraRight * moveX;

        if (moveDirection.magnitude > 0.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                targetRotation, 
                turnSpeed * Time.deltaTime);
        }

        // 3. Handle Charge Jump Input
        if (isGrounded)
        {
            // While holding Space, accumulate charge up to maxChargeTime
            if (Input.GetKey(KeyCode.Space))
            {
                currentJumpCharge += Time.deltaTime;
                currentJumpCharge = Mathf.Min(currentJumpCharge, maxChargeTime);
            }

            // When releasing Space, request the jump using the calculated charge proportion
            if (Input.GetKeyUp(KeyCode.Space))
            {
                jumpRequested = true;
            }
        }
        else
        {
            // Reset charge if we leave the ground without jumping normally
            if (Input.GetKeyUp(KeyCode.Space))
            {
                currentJumpCharge = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
     
        if (jumpRequested)
        {
            // Calculate total jump force based on how long Space was held
            float chargeRatio = currentJumpCharge / maxChargeTime; // 0 to 1
            float finalJumpForce = jumpForce + (maxChargeBonus * chargeRatio);

            Jump(finalJumpForce);

            // Reset charge and request flag
            currentJumpCharge = 0f;
            jumpRequested = false;
        }
     
        // The second jump, in mid-air
        if (doubleJumpRequested)
        {
            Jump(doubleJumpForce);
            doubleJumpRequested = false;
        }
    }
     
    private void Jump(float force)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f,
                                        rb.linearVelocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.VelocityChange);
    }

    private void MovePlayer()
    {
        float currentSpeed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? runSpeed : walkSpeed;

        // Calculate velocity based on current forward vector
        Vector3 targetVelocity = moveDirection * currentSpeed * SpeedMultiplier;

        // Apply movement while preserving vertical velocity (gravity/jumping)
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

    // Tries to jump a second time in mid-air.
    // Returns true if the double jump really happened.
    public bool TryDoubleJump()
    {
        if (powerUps == null || !powerUps.IsJumpActive) return false;

        // Not allowed on the ground, or if it was already used
        if (isGrounded || hasDoubleJumped) return false;
     
        doubleJumpRequested = true;
        hasDoubleJumped = true;
        return true;
    }
}