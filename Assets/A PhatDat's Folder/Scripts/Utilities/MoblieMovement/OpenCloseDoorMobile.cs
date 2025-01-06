using UnityEngine;

public class OpenCloseDoorMobile : MonoBehaviour
{
    public AudioSource audioSource;      // Đối tượng AudioSource để phát âm thanh khi mở/đóng cửa
    public AudioClip openSound;         // Âm thanh khi mở cửa
    public AudioClip closeSound;        // Âm thanh khi đóng cửa
    public bool inReach, toggle;        // inReach: Kiểm tra xem người chơi có ở gần cửa không; toggle: Biến trạng thái cửa (mở hay đóng)

    public Animator door;               // Animator để điều khiển các hiệu ứng hoạt hình của cửa

    // Thêm biến để đánh dấu cửa đang được nhắm đến
    public static OpenCloseDoorMobile currentTarget;  // Biến static để giữ cửa hiện tại mà người chơi đang tương tác

    // Phương thức Start() được gọi khi đối tượng được khởi tạo
    void Start()
    {
        // Kiểm tra xem UIManager có tồn tại không, nếu có thì ẩn UI toggle
        if (UIManager.Instance != null)
        {
            UIManager.Instance.hideToggleUI();
        }
        else
        {
            Debug.LogWarning("UIManager không có trong ứng dụng");  // In cảnh báo nếu không tìm thấy UIManager
        }

        inReach = false;  // Đặt giá trị ban đầu cho inReach là false
        toggle = true;     // Đặt giá trị ban đầu cho toggle là true (cửa đóng)
    }

    // Phương thức OnTriggerStay được gọi khi đối tượng va chạm với collider của cửa
    public void OnTriggerStay(Collider other)
    {
        // Kiểm tra xem đối tượng va chạm có tag là "Reach" không (người chơi trong phạm vi tương tác)
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;  // Đánh dấu người chơi đang trong phạm vi
            UIManager.Instance.showToggleUI();  // Hiển thị UI toggle
            // Đặt cửa hiện tại là cửa này
            currentTarget = this;
        }
    }

    // Phương thức OnTriggerExit được gọi khi đối tượng rời khỏi collider của cửa
    public void OnTriggerExit(Collider other)
    {
        // Kiểm tra xem đối tượng rời khỏi có tag là "Reach" không (người chơi rời khỏi phạm vi tương tác)
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;  // Đánh dấu người chơi đã ra khỏi phạm vi
            UIManager.Instance.hideToggleUI();  // Ẩn UI toggle
            // Đặt lại target nếu ra khỏi vùng tương tác
            if (currentTarget == this)
            {
                currentTarget = null;
            }
        }
    }

    // Phương thức HandleInteraction được gọi khi người chơi tương tác với cửa
    public void HandleInteraction()
    {
        if (inReach)  // Kiểm tra xem người chơi có đang trong phạm vi tương tác hay không
        {
            // Nếu toggle là true (cửa đang đóng), gọi phương thức mở cửa
            if (toggle)
                DoorOpened();
            else  // Nếu toggle là false (cửa đang mở), gọi phương thức đóng cửa
                DoorCloseed();

            UIManager.Instance.hideToggleUI();  // Ẩn UI toggle sau khi thực hiện tương tác
        }
    }

    // Phương thức mở cửa
    public void DoorOpened()
    {
        door.ResetTrigger("closed");  // Hủy bỏ trigger đóng cửa
        door.SetTrigger("opened");    // Đặt trigger mở cửa
        audioSource.PlayOneShot(openSound);  // Phát âm thanh mở cửa
        toggle = false;  // Đặt toggle thành false (cửa đã mở)
    }

    // Phương thức đóng cửa
    public void DoorCloseed()
    {
        door.ResetTrigger("opened");  // Hủy bỏ trigger mở cửa
        door.SetTrigger("closed");    // Đặt trigger đóng cửa
        audioSource.PlayOneShot(closeSound);  // Phát âm thanh đóng cửa
        toggle = true;  // Đặt toggle thành true (cửa đã đóng)
    }
}
