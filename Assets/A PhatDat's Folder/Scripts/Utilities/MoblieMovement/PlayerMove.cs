using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [Header("Player Movement")]
    public FixedJoystick joystick;           // Joystick di chuyển
    public float walkSpeed = 5f;             // Tốc độ đi bộ
    public float sprintSpeed = 10f;          // Tốc độ chạy nhanh
    public float crouchSpeed = 2f;           // Tốc độ cúi

    [Space(10)]
    public float gravity = -9.81f;           // Trọng lực
    public float crouchScale = 0.5f;         // Tỉ lệ khi cúi
    private float originalScaleY;            // Tỉ lệ ban đầu

    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainRate = 15f;
    [SerializeField] private float staminaRegenRate = 10f;
    [SerializeField] private float staminaRegenDelay = 2f;
    [SerializeField] private Image staminaBar; // UI stamina

    private float currentStamina;
    private float staminaRegenTimer;
    private bool isOutOfStamina = false;

    [Header("Player Grounded")]
    public bool grounded = true;
    public float groundedOffset = -0.14f;
    public float groundedRadius = 0.5f;
    public LayerMask groundLayers;

    [SerializeField] private Transform PlayerBody;
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
        UpdateStaminaUI(); // Khởi tạo thanh stamina
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

    public void Move()
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
                currentStamina = Mathf.Max(currentStamina, 0);
            }
            else
            {
                isOutOfStamina = true;
            }

            staminaRegenTimer = 0f;
        }

        // Di chuyển nhân vật qua joystick
        Vector3 move = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
        move = PlayerBody.transform.TransformDirection(move); // Chuyển sang tọa độ nhân vật
        controller.Move(move.normalized * targetSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void ManageStamina()
    {
        if (!pressedSprint && currentStamina < maxStamina)
        {
            staminaRegenTimer += Time.deltaTime;

            if (staminaRegenTimer >= staminaRegenDelay)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, maxStamina);

                if (currentStamina > 0)
                {
                    isOutOfStamina = false; // Cho phép chạy lại
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
