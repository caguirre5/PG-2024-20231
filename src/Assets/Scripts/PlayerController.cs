using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float rotationSpeed = 720f; // Speed of rotation in degrees per second

    private float defaultMoveSpeed; // Guardar la velocidad original

    private CharacterController controller;
    [SerializeField] private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;

    private Quaternion moveRotation;

    private float attackHoldTime = 0f;
    private float maxHoldTimeForAttack2 = 0.5f; // Tiempo necesario para pasar a attack2

    // Weapons
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private GameObject shield;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveRotation = Quaternion.Euler(0, 45, 0); // Rotate movement by 45 degrees on the Y axis
        sword.SetActive(false);
        shield.SetActive(false);

        defaultMoveSpeed = moveSpeed; // Guardar la velocidad original al inicio
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Ensure the player sticks to the ground
            animator.SetBool("isJumping", false);
        }

        // Aumentar la velocidad al presionar Shift y activar la animación de isRunningFaster
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveSpeed = defaultMoveSpeed * 2; // Doblar la velocidad
            animator.SetBool("isRunningFaster", true); // Activar la animación de correr rápido
            animator.SetBool("isRunning", false);
        }
        else
        {
            moveSpeed = defaultMoveSpeed; // Restaurar la velocidad original
            animator.SetBool("isRunningFaster", false); // Desactivar la animación de correr rápido

        }

        // Get input for movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ);

        // Rotate the movement vector for isometric view
        move = moveRotation * move;

        if (move != Vector3.zero)
        {
            // Calculate the target rotation based on the direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(move);
            // Smoothly rotate towards the target direction
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // Move the character
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Check for jump input and apply jump force if grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
            animator.SetTrigger("Jump");
        }

        // Handle attack input
        if (Input.GetMouseButton(0)) // Detect if the left mouse button is held down
        {
            attackHoldTime += Time.deltaTime;
            HandleAttack();
        }
        else if (Input.GetMouseButtonUp(0)) // Detect when the left mouse button is released
        {
            ResetAttack();
        }

        // Handle block input
        if (Input.GetMouseButtonDown(1))
        {
            StartBlocking();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            StopBlocking();
        }

        // Apply gravity to the player
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleAttack()
    {
        sword.SetActive(true);
        if (attackHoldTime >= maxHoldTimeForAttack2)
        {
            animator.SetTrigger("attack2");
        }
        else
        {
            animator.SetTrigger("attack1");
        }
    }

    void ResetAttack()
    {
        attackHoldTime = 0f;
        animator.ResetTrigger("attack1");
        animator.ResetTrigger("attack2");
        sword.SetActive(false);
    }

    void StartBlocking()
    {
        shield.SetActive(true);
        animator.SetBool("isBlocking", true);
        moveSpeed /= 2;
    }

    void StopBlocking()
    {
        shield.SetActive(false);
        animator.SetBool("isBlocking", false);
        moveSpeed *= 2;
    }
}
