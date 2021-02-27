using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float walkSpeed = 12f;
    [SerializeField] float runSpeed = 16f;
    [SerializeField] float jumpHeight = 1;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float groundDistance = 0.3f;
    [SerializeField] LayerMask groundMask;

    float speed;

    CharacterController controller;
    Transform groundCheck;

    Vector3 movementDirection;
    Vector3 velocity;
    bool isGrounded;

    InputManager inputManager;
    Rigidbody rb;

    RaycastHit hit;

    Animator gameAnimator;

    public delegate void Die();
    public static event Die OnDie;

    void Awake()
    {
        gameAnimator = transform.parent.GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        groundCheck = transform.GetChild(1);
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (inputManager.shift)
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        movementDirection = transform.right * inputManager.horizontal + transform.forward * inputManager.vertical;

        movementDirection.Normalize();

        controller.Move((movementDirection + PlatformMovement()) * speed * Time.deltaTime);

        if (isGrounded && inputManager.jump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    Vector3 PlatformMovement()
    {
        if (Physics.Raycast(groundCheck.position, transform.TransformDirection(Vector3.down), out hit, 0.2f))
        {
            if (hit.transform.GetComponent<MovingObject>())
            {
                return hit.transform.GetComponent<MovingObject>().deltaMovement * speed;
            }
            if (hit.transform.GetComponent<ToggleMovingObject>())
            {
                return hit.transform.GetComponent<ToggleMovingObject>().deltaMovement * speed;
            }
            if (hit.transform.CompareTag("Danger"))
            {
                Died();
            }
            return Vector3.zero;
        }
        else
        {
            return Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Danger"))
        {
            Died();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Danger"))
        {
            Died();
        }
    }

    void Died()
    {
        if (GetComponent<TimeBody>().isRewinding == false)
            gameAnimator.SetTrigger("Died");
        Invoke(nameof(InvokeDie), 0.5f);
    }

    void InvokeDie()
    {
        gameAnimator.ResetTrigger("Died");
        OnDie?.Invoke();
    }

}
