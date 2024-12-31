// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class TouchController : MonoBehaviour
// {
//     public FixedTouchField _FixedTouchField;
//     public CameraLook _CameraLook;
//     public PlayerMove _PlayerMove;
//     public FixedButton CrouchButton;  // Nút để cúi
//     public FixedButton SprintButton; // Nút để chạy nhanh
//     public FixedButton InteractButton; // Nút để tương tác

//     void Update()
//     {
//         _CameraLook.LockAxis = _FixedTouchField.TouchDist;

//         //Điều khiển trạng thái cúi và chạy nhanh của nhân vật
//         if (_PlayerMove != null)
//         {
//             _PlayerMove.pressedCrouch = CrouchButton != null && CrouchButton.Pressed;
//             _PlayerMove.pressedSprint = SprintButton != null && SprintButton.Pressed;
//         }
//     }
//     private void HandleInteract()
//     {
       
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public FixedTouchField _FixedTouchField;
    public CameraLook _CameraLook;
    public PlayerMove _PlayerMove;
    public FixedButton CrouchButton;  // Nút để cúi
    public FixedButton SprintButton; // Nút để chạy nhanh
    public FixedButton InteractButton; // Nút để tương tác

    // Tham chiếu đến OpenCloseDoor1
    public OpenCloseDoorMobile doorController;

    void Update()
    {
        // Xoay camera dựa trên touch field
        _CameraLook.LockAxis = _FixedTouchField.TouchDist;

        // Điều khiển trạng thái cúi và chạy nhanh của nhân vật
        if (_PlayerMove != null)
        {
            _PlayerMove.pressedCrouch = CrouchButton != null && CrouchButton.Pressed;
            _PlayerMove.pressedSprint = SprintButton != null && SprintButton.Pressed;
        }

        // Xử lý khi nhấn nút Interact
        if (InteractButton != null && InteractButton.Pressed)
        {
            if (doorController != null)
            {
                doorController.HandleInteraction(); // Gọi phương thức HandleInteraction trong OpenCloseDoor1
            }
        }
    }
}
