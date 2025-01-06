using UnityEngine;

public class ItemPickupMobile : MonoBehaviour
{
    // Các biến được khai báo trong script
    public GameObject Item;          // Món đồ mà người chơi có thể nhặt
    public AudioSource pickup;       // Đối tượng phát âm thanh nhặt đồ
    public AudioClip pickupSound;    // Âm thanh phát ra khi nhặt đồ
    public Transform hand;           // Biến lưu trữ vị trí của tay (chỗ đồ sẽ được đặt)
    public Vector3 pos;              // Vị trí đồ sẽ được đặt khi nhặt
    public Quaternion rot;           // Góc quay của đồ khi nhặt
    public bool inReach;             // Kiểm tra xem người chơi có trong tầm nhặt đồ không
    public bool followhand;          // Kiểm tra xem đồ có đi theo tay người chơi không
    public EquipSlot Slots;          // Vị trí của các ô đồ trong tay người chơi
    public ReachItem CurrentItem;    // Món đồ hiện tại mà người chơi có thể nhặt
    public FixedButton interactButton;  // Nút tương tác trên giao diện di động

    // Phương thức gọi khi người chơi va chạm với collider của món đồ
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")  // Kiểm tra nếu đối tượng có tag "Reach"
        {
            UIManager.Instance.showToggleUI(); // Hiển thị UI tương tác
            inReach = true;  // Đánh dấu là trong tầm nhặt
        }
    }

    // Phương thức gọi khi người chơi rời khỏi collider của món đồ
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")  // Kiểm tra nếu đối tượng có tag "Reach"
        {
            UIManager.Instance.hideToggleUI();  // Ẩn UI tương tác
            inReach = false;  // Đánh dấu là không còn trong tầm nhặt
        }
    }

    // Phương thức khởi tạo ban đầu
    private void Start()
    {
        if (Item == null)  // Kiểm tra nếu món đồ chưa được gán
        {
            Debug.LogError("Item chưa được khởi tạo trong Inspector hoặc script.");
        }

        if (UIManager.Instance != null)  // Kiểm tra UIManager có tồn tại không
        {
            UIManager.Instance.hideToggleUI();  // Ẩn UI khi bắt đầu
        }
        else
        {
            Debug.LogWarning("UIManager không có trong ứng dụng");  // Cảnh báo nếu không tìm thấy UIManager
        }

        pos = hand.transform.position;  // Lưu vị trí của tay
        followhand = false;  // Đặt mặc định là không theo tay
        Slots = GameObject.FindGameObjectWithTag("RightHand").GetComponent<EquipSlot>();  // Lấy đối tượng EquipSlot từ tay phải
        CurrentItem = GameObject.FindGameObjectWithTag("Reach").GetComponent<ReachItem>();  // Lấy đối tượng ReachItem
    }

    // Phương thức Update chạy liên tục trong mỗi frame
    public void Update()
    {
        if (inReach && !followhand)  // Kiểm tra nếu đối tượng ở trong tầm nhặt và chưa theo tay
        {
            if (interactButton != null && interactButton.Pressed)  // Kiểm tra nếu nút tương tác được nhấn
            {
                if (GetItemIntoSlot())  // Thử đưa món đồ vào một ô trống
                {
                    UIManager.Instance.hideToggleUI();  // Ẩn UI khi nhặt đồ
                    inReach = false;  // Đánh dấu không còn trong tầm nhặt
                    PlayPickUpSound();  // Phát âm thanh nhặt đồ
                    Item.transform.localPosition = pos;  // Đặt vị trí của món đồ vào vị trí tay
                    followhand = true;  // Đánh dấu món đồ sẽ theo tay người chơi
                    OnHand temp = CurrentItem.CurrentItem.GetComponent<OnHand>();  // Lấy đối tượng OnHand
                    if (temp != null)
                    {
                        temp.onHand = followhand;  // Đánh dấu món đồ theo tay
                    }
                    if (Item != null)
                    {
                        Item.SetActive(false);  // Ẩn món đồ khi đã nhặt
                    }
                    else
                    {
                        Debug.LogWarning("Item chưa được gán giá trị!");  // Cảnh báo nếu món đồ không được gán giá trị
                    }
                }
            }
        }
        else if (followhand)  // Nếu món đồ đang theo tay
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) { Item.SetActive(true); }  // Nếu nhấn phím số 1, hiện lại món đồ
            transform.position = hand.position;  // Đặt vị trí của món đồ theo tay
            transform.rotation = hand.rotation;  // Đặt góc quay của món đồ theo tay
        }
    }

    // Phương thức đưa món đồ vào một ô trống trong tay người chơi
    private bool GetItemIntoSlot()
    {
        for (int i = 0; i < Slots.ItemInSlot.Length; i++)  // Duyệt qua tất cả các ô đồ
        {
            if (!Slots.IsFull[i])  // Nếu ô đồ chưa đầy
            {
                Slots.ItemInSlot[i] = Item;  // Gán món đồ vào ô
                Slots.IsFull[i] = true;  // Đánh dấu ô đồ đã đầy
                return true;  // Trả về true nếu thành công
            }
        }
        return false;  // Trả về false nếu không tìm thấy ô trống
    }

    // Phương thức phát âm thanh nhặt đồ
    void PlayPickUpSound()
    {
        pickup.PlayOneShot(pickupSound);  // Phát âm thanh nhặt đồ
    }
}
