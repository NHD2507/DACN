using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    // Các biến tham chiếu đến các đối tượng giao diện người dùng và các đối tượng điều khiển trong trò chơi
    public FixedTouchField _FixedTouchField;  // Điều khiển cảm ứng cho việc di chuyển
    public CameraLook _CameraLook;  // Điều khiển việc quay camera
    public PlayerMove _PlayerMove;  // Điều khiển chuyển động của nhân vật
    public FixedButton CrouchButton;  // Nút ngồi
    public FixedButton SprintButton;  // Nút chạy
    public FixedButton InteractButton;  // Nút tương tác
    public FixedButton SwitchItemButton;  // Nút chuyển đồ
    public FixedButton UseItemButton;  // Nút sử dụng đồ
    public FixedButton EscButton;  // Nút thoát

    // Các đối tượng giao diện
    public GameObject MovePanel;  // Bảng điều khiển di chuyển
    public GameObject UtilitiesPanel;  // Bảng công cụ

    public EscMenuUI escMenuUI;  // Menu thoát

    // Các trạng thái của nút
    private bool interactButtonPressed;
    private bool switchItemButtonPressed;
    private bool useItemButtonPressed;

    // ID các ngón tay dùng để điều khiển các phần khác nhau
    private int moveTouchId = -1;
    private int cameraTouchId = -1;

    void Start()
    {
        // Tìm đối tượng EscMenuUI trong cảnh và gán vào biến escMenuUI
        escMenuUI = FindAnyObjectByType<EscMenuUI>();

        // Tìm tất cả các đối tượng ItemPickupMobile và gán nút tương tác cho chúng
        ItemPickupMobile[] itemPickups = FindObjectsOfType<ItemPickupMobile>();
        foreach (ItemPickupMobile itemPickup in itemPickups)
        {
            itemPickup.interactButton = InteractButton;
        }

        // Cập nhật trạng thái hiển thị các nút
        UpdateButtonVisibility();
    }

    void Update()
    {
        // Duyệt qua tất cả các điểm chạm trên màn hình
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            // Nếu chưa có ngón tay nào dùng để di chuyển và chạm vào bảng di chuyển, gán ID ngón tay
            if (moveTouchId == -1 && MovePanel.activeInHierarchy && IsTouchInPanel(touch.position, MovePanel))
            {
                moveTouchId = touch.fingerId;
            }

            // Nếu chưa có ngón tay nào dùng để điều khiển camera và chạm vào bảng công cụ, gán ID ngón tay
            if (cameraTouchId == -1 && UtilitiesPanel.activeInHierarchy && IsTouchInPanel(touch.position, UtilitiesPanel))
            {
                cameraTouchId = touch.fingerId;
            }

            // Nếu ngón tay này đang điều khiển di chuyển, gọi phương thức Move()
            if (touch.fingerId == moveTouchId)
            {
                _PlayerMove.Move();
            }

            // Nếu ngón tay này đang điều khiển camera, cho phép quay camera
            if (touch.fingerId == cameraTouchId)
            {
                _CameraLook.allowCameraRotation = true;
                _CameraLook.LockAxis = _FixedTouchField.TouchDist;
            }
        }

        // Kiểm tra khi ngón tay rời khỏi màn hình hoặc bị hủy bỏ
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    if (touch.fingerId == moveTouchId)
                    {
                        moveTouchId = -1;  // Dừng điều khiển di chuyển
                    }
                    if (touch.fingerId == cameraTouchId)
                    {
                        cameraTouchId = -1;  // Dừng điều khiển camera
                        _CameraLook.allowCameraRotation = false;
                    }
                }
            }
        }

        // Kiểm tra và cập nhật trạng thái các nút
        if (_PlayerMove != null)
        {
            _PlayerMove.pressedCrouch = CrouchButton != null && CrouchButton.Pressed;
            _PlayerMove.pressedSprint = SprintButton != null && SprintButton.Pressed;
        }

        HandleInteractButton();  // Xử lý nút tương tác
        HandleSwitchItemButton();  // Xử lý nút chuyển đồ
        HandleUseItemButton();  // Xử lý nút sử dụng đồ

        // Kiểm tra nếu nút Esc được nhấn để mở menu thoát
        if (EscButton.Pressed)
        {
            escMenuUI.Pause();
        }

        UpdateButtonVisibility();  // Cập nhật trạng thái hiển thị nút
    }

    // Kiểm tra xem vị trí chạm có nằm trong bảng điều khiển không
    private bool IsTouchInPanel(Vector2 touchPosition, GameObject panel)
    {
        RectTransform panelRectTransform = panel.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(panelRectTransform, touchPosition, null);
    }

    // Xử lý hành động khi nút tương tác được nhấn
    private void HandleInteractButton()
    {
        if (InteractButton != null && InteractButton.Pressed && !interactButtonPressed)
        {
            interactButtonPressed = true;

            // Nếu có cửa, thực hiện hành động tương tác với cửa
            if (OpenCloseDoorMobile.currentTarget != null)
            {
                OpenCloseDoorMobile.currentTarget.HandleInteraction();
            }

            // Nếu có ánh sáng, bật hoặc tắt ánh sáng
            // LightOnOffMobile lightScript = FindObjectOfType<LightOnOffMobile>();
            // if (lightScript != null && lightScript.PlayerInZone == true)
            // {
            //     Debug.Log("Gọi hàm ToggleLight() từ LightOnOffMobile.");
            //     lightScript.ToggleLight();
            // }
            // else
            // {
            //     Debug.LogError("Không tìm thấy LightOnOffMobile trong Scene.");
            // }
            if (LightOnOffMobile.currentTarget != null)
            {
                LightOnOffMobile.currentTarget.ToggleLight();
            }

            // Nếu có các vật phẩm có thể nhặt, thực hiện hành động với chúng
            ItemPickupMobile[] itemPickups = FindObjectsOfType<ItemPickupMobile>();
            foreach (ItemPickupMobile itemPickup in itemPickups)
            {
                if (itemPickup.inReach)
                {
                    itemPickup.Update();
                }
            }
        }
        if (InteractButton != null && !InteractButton.Pressed)
        {
            interactButtonPressed = false;
        }
    }

    // Xử lý hành động khi nút chuyển đồ được nhấn
    private void HandleSwitchItemButton()
    {
        if (SwitchItemButton != null && SwitchItemButton.Pressed && !switchItemButtonPressed)
        {
            switchItemButtonPressed = true;
            Equip equip = FindObjectOfType<Equip>();
            if (equip != null)
            {
                equip.SwitchToNextItem();  // Chuyển sang vật phẩm tiếp theo
            }
            else
            {
                Debug.LogWarning("Không tìm thấy Equip component!");
            }
        }
        if (SwitchItemButton != null && !SwitchItemButton.Pressed)
        {
            switchItemButtonPressed = false;
        }
    }

    // Xử lý hành động khi nút sử dụng đồ được nhấn
    private void HandleUseItemButton()
    {
        if (UseItemButton != null && UseItemButton.Pressed && !useItemButtonPressed)
        {
            useItemButtonPressed = true;
            Equip equip = FindObjectOfType<Equip>();
            if (equip != null && equip.CurrentObj != null)
            {
                // Nếu vật phẩm là đèn pin, bật/tắt đèn
                FlashLight flashLight = equip.CurrentObj.GetComponent<FlashLight>();
                if (flashLight != null)
                {
                    flashLight.ToggleLight();
                }

                // Nếu vật phẩm là máy ảnh, chụp ảnh
                Photograph photograph = equip.CurrentObj.GetComponent<Photograph>();
                if (photograph != null)
                {
                    photograph.TakePhoto();

                    // Gọi phương thức CameraContact() từ TurnToNormal
                    TurnToNormal turnToNormalScript = FindObjectOfType<TurnToNormal>();
                    if (turnToNormalScript != null)
                    {
                        // Nếu sử dụng máy ảnh, chuyển anomaly trở lại bình thường
                        turnToNormalScript.CameraContact();  // Xử lý chuyển đổi anomaly trong TurnToNormal
                    }
                }
            }
        }
        if (UseItemButton != null && !UseItemButton.Pressed)
        {
            useItemButtonPressed = false;
        }
    }

    // Cập nhật trạng thái hiển thị các nút
    private void UpdateButtonVisibility()
    {
        Equip equip = FindObjectOfType<Equip>();
        EquipSlot slot = GameObject.FindGameObjectWithTag("RightHand").GetComponent<EquipSlot>();
        bool hasItems = false;
        bool holdingItem = (equip != null && equip.CurrentObj != null);

        // Kiểm tra xem có vật phẩm nào trong tay không
        for (int i = 0; i < slot.ItemInSlot.Length; i++)
        {
            if (slot.ItemInSlot[i] != null)
            {
                hasItems = true;
                break;
            }
        }

        // Hiển thị hoặc ẩn các nút chuyển đồ và sử dụng đồ
        if (SwitchItemButton != null)
        {
            SwitchItemButton.gameObject.SetActive(hasItems);
        }

        if (UseItemButton != null)
        {
            UseItemButton.gameObject.SetActive(holdingItem);
        }
    }
}
