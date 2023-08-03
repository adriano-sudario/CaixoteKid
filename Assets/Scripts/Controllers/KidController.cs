using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class KidController : MonoBehaviour
{
    public float speed = 10;
    public float jumpForce = 10;

    private readonly float groundDistanceTolerance = .15f;

    private Vector3 direction;
    private Rigidbody rigidBody;
    private Animator animator;
    private float colliderHeight;
    private float rotationSpeed = 720;
    private CapsuleCollider capsuleCollider;
    private bool shouldJump;
    private bool isHoldingShift;

    private bool IsOnGround => Physics.Raycast(
            transform.position + (transform.up * colliderHeight * .5f),
            -transform.up,
            (colliderHeight * .5f) + groundDistanceTolerance);

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        colliderHeight = capsuleCollider.height;
    }

    void Update()
    {
        var horizontalAxis = Input.GetAxis("Horizontal");
        var depthAxis = Input.GetAxis("Vertical");
        direction = new Vector3(horizontalAxis, 0, depthAxis);
        direction.Normalize();

        if (!shouldJump)
            shouldJump = IsOnGround && Input.GetKeyDown(KeyCode.Space);

        isHoldingShift = Input.GetKey(KeyCode.LeftShift);
    }

    void FixedUpdate()
    {
        var isMoving = direction != Vector3.zero;

        if (isMoving)
        {
            animator.SetBool("IsRunning", isHoldingShift);
            var isRunning = Input.GetKey(KeyCode.LeftShift);
            transform.Translate(direction * (isRunning ? speed + 5 : speed) * Time.deltaTime, Space.World);

            var toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        animator.SetBool("IsMoving", isMoving);

        if (shouldJump)
        {
            shouldJump = false;
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
