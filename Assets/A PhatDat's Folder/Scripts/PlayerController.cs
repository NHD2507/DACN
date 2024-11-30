﻿//using Photon.Pun;
//using UnityEngine;

//[RequireComponent(typeof(CharacterController))]
//public class PlayerController : MonoBehaviourPunCallbacks
//{
//    [Header("Movement Settings")]
//    [SerializeField] private float walkSpeed = 5f;
//    [SerializeField] private float runSpeed = 10f;
//    [SerializeField] private float jumpHeight = 2f;
//    [SerializeField] private float gravity = -9.81f;

//    [Header("Mouse Look Settings")]
//    [SerializeField] private float mouseSensitivity = 100f;
//    [SerializeField] private Transform cameraTransform;

//    [Header("Ground Check")]
//    [SerializeField] private Transform groundCheck;
//    [SerializeField] private float groundDistance = 0.4f;
//    [SerializeField] private LayerMask groundMask;

//    private CharacterController characterController;
//    private Vector3 velocity;
//    private bool isGrounded;
//    private float xRotation = 0f;

//    private void Start()
//    {
//        characterController = GetComponent<CharacterController>();

//        // Chỉ kích hoạt camera và điều khiển trên client local
//        if (photonView.IsMine)
//        {
//            Cursor.lockState = CursorLockMode.Locked;
//        }
//        else
//        {
//            cameraTransform.gameObject.SetActive(false);
//        }
//    }

//    private void Update()
//    {
//        if (photonView.IsMine) // Chỉ cho phép điều khiển nếu là chủ sở hữu (player local)
//        {
//            HandleMovement();
//            HandleMouseLook();
//        }
//    }

//    private void HandleMovement()
//    {
//        // Kiểm tra nhân vật có đang trên mặt đất
//        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

//        if (isGrounded && velocity.y < 0)
//        {
//            velocity.y = -2f; // Giữ nhân vật "dính" mặt đất
//        }

//        // Lấy đầu vào bàn phím
//        float moveX = Input.GetAxis("Horizontal");
//        float moveZ = Input.GetAxis("Vertical");

//        Vector3 move = transform.right * moveX + transform.forward * moveZ;

//        // Kiểm tra xem người chơi có đang chạy
//        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

//        characterController.Move(move * currentSpeed * Time.deltaTime);

//        // Nhảy
//        if (Input.GetButtonDown("Jump") && isGrounded)
//        {
//            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
//        }

//        // Áp dụng trọng lực
//        velocity.y += gravity * Time.deltaTime;
//        characterController.Move(velocity * Time.deltaTime);
//    }

//    private void HandleMouseLook()
//    {
//        // Lấy đầu vào chuột
//        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
//        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

//        // Xử lý xoay dọc (camera)
//        xRotation -= mouseY;
//        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

//        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

//        // Xử lý xoay ngang (nhân vật)
//        transform.Rotate(Vector3.up * mouseX);
//    }
//}


using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviourPunCallbacks
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Mouse Look Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform cameraTransform;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (photonView.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            cameraTransform.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            HandleMovement();
            HandleMouseLook();
        }
    }

    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset falling velocity
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        characterController.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, -50f); // Limit falling speed
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
