//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CameraLook : MonoBehaviour
//{
//    private float XMove;
//    private float YMove;
//    private float XRotation;
//    [SerializeField] private Transform PlayerBody;
//    public Vector2 LockAxis;
//    public float Sensivity = 5f;

//    public GameObject UtilitiesPanel; // Tham chiếu đến panel quay camera

//    void Update()
//    {
//        // Kiểm tra nếu UtilitiesPanel đang hoạt động và chỉ xử lý quay camera khi chạm vào panel này
//        if (UtilitiesPanel.activeInHierarchy && Input.touchCount == 1)  // Chỉ xử lý khi có 1 ngón tay và panel quay camera đang hiển thị
//        {
//            Touch touch = Input.GetTouch(0);
//            Vector2 touchDelta = touch.deltaPosition;  // Sự thay đổi vị trí cảm ứng

//            // Quay camera theo cảm ứng
//            XMove = touchDelta.x * Sensivity * Time.deltaTime;
//            YMove = touchDelta.y * Sensivity * Time.deltaTime;

//            XRotation -= YMove;
//            XRotation = Mathf.Clamp(XRotation, -90f, 90f);

//            transform.localRotation = Quaternion.Euler(XRotation, 0f, 0f);
//            PlayerBody.Rotate(Vector3.up * XMove);
//        }
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    private float XMove;
    private float YMove;
    private float XRotation;
    [SerializeField] private Transform PlayerBody;
    public Vector2 LockAxis;
    public float Sensivity = 5f;

    // Biến để bật tắt quay camera
    public bool allowCameraRotation = false;

    void Update()
    {
        if (allowCameraRotation && Input.touchCount == 1) // Chỉ xử lý khi được phép và có 1 ngón tay
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchDelta = touch.deltaPosition;  // Sự thay đổi vị trí cảm ứng

            // Quay camera theo cảm ứng
            XMove = touchDelta.x * Sensivity * Time.deltaTime;
            YMove = touchDelta.y * Sensivity * Time.deltaTime;

            XRotation -= YMove;
            XRotation = Mathf.Clamp(XRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(XRotation, 0f, 0f);
            PlayerBody.Rotate(Vector3.up * XMove);
        }
    }
}

