using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [Header("Player Movement")]
    public FixedJoystick joystick;
    [Tooltip("Walk speed of the character in m/s")]
    public float walkSpeed = 5f;
    [Tooltip("Sprint speed of the character in m/s")]
    public float sprintSpeed = 10f;
    [Tooltip("Crouch speed of the character in m/s")]
    public float crouchSpeed = 2f;

    [Space(10)]
    [Tooltip("The character's own gravity value. Default is -9.81f")]
    public float gravity = -9.81f;
    public float crouchScale = 0.5f;
    private float originalScaleY;

    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainRate = 15f;
    [SerializeField] private float staminaRegenRate = 10f;
    [SerializeField] private float staminaRegenDelay = 2f;
    [SerializeField] private Image staminaBar; // UI bar for stamina

    private float currentStamina;
    private float staminaRegenTimer;
    private bool isOutOfStamina = false;

    [Header("Player Grounded")]
    public bool grounded = true;
    public float groundedOffset = -0.14f;
    public float groundedRadius = 0.5f;
    public LayerMask groundLayers;

    private CharacterController controller;
    private Vector3 velocity;

    [Header("Input States")]
    public bool pressedCrouch;
    public bool pressedSprint;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        originalScaleY = transform.localScale.y;
        currentStamina = maxStamina;
        UpdateStaminaUI(); // Initialize stamina UI
    }

    private void Update()
    {
        GroundCheck();
        Move();
        ApplyGravity();
        ManageStamina();
    }

    private void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + groundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);

        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

private void Move()
{
    float targetSpeed = walkSpeed;

    // Xử lý cúi
    if (pressedCrouch)
    {
        targetSpeed = crouchSpeed;
        transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
    }
    else
    {
        transform.localScale = new Vector3(transform.localScale.x, originalScaleY, transform.localScale.z);
    }

    // Xử lý chạy nhanh
    if (pressedSprint && !pressedCrouch)
    {
        if (currentStamina > 0)
        {
            targetSpeed = sprintSpeed;
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0); // Đảm bảo stamina không âm
        }
        else
        {
            isOutOfStamina = true; // Gắn cờ hết stamina
        }

        staminaRegenTimer = 0f; // Reset bộ đếm hồi stamina khi đang chạy
    }

    // Di chuyển nhân vật
    Vector3 move = transform.right * joystick.Horizontal + transform.forward * joystick.Vertical;
    controller.Move(move.normalized * (targetSpeed * Time.deltaTime));
}

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void ManageStamina()
    {
        // Nếu không nhấn nút chạy nhanh và stamina chưa đầy
        if (!pressedSprint && currentStamina < maxStamina)
        {
            staminaRegenTimer += Time.deltaTime;

            if (staminaRegenTimer >= staminaRegenDelay)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, maxStamina); // Đảm bảo stamina không vượt quá max

                if (currentStamina > 0)
                {
                    isOutOfStamina = false; // Cho phép chạy lại nếu stamina hồi lại một phần
                }
            }
        }

        UpdateStaminaUI();
    }

    private void UpdateStaminaUI()
    {
        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }
    }
}