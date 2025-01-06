using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTimeTrigger : MonoBehaviour
{
    // Các biến được khai báo
    [SerializeField] private Cooldown _cooldown;  // Đối tượng Cooldown, giúp điều khiển thời gian chờ giữa các lần kích hoạt
    public GameObject AllAnomalys;                // Đối tượng chứa tất cả các anomaly (hiện tượng bất thường)
    public RoomID[] rooms;                       // Mảng các phòng (RoomID) có thể kích hoạt
    public int trigger;                          // Biến xác định xác suất kích hoạt
    public int triggerRate;                      // Tỷ lệ xác suất (từ 0 đến 100) để kích hoạt phòng

    // Phương thức Awake() được gọi khi đối tượng được khởi tạo
    void Awake()
    {
        Debug.Log("started time trigger");  // In thông báo khi bắt đầu trigger
        rooms = AllAnomalys.GetComponentsInChildren<RoomID>();  // Lấy tất cả các đối tượng con có kiểu RoomID từ AllAnomalys
        //triggerRate = 5; // Tỷ lệ kích hoạt có thể được gán trực tiếp
    }

    // Phương thức Update() chạy mỗi frame
    void Update()
    {
        if (_cooldown.IsCoolingDown) return;  // Nếu đang trong thời gian chờ, không làm gì và thoát ra

        // Chọn một số ngẫu nhiên giữa 0 và 100
        trigger = Random.Range(0, 100);
        // Nếu số ngẫu nhiên nhỏ hơn hoặc bằng tỷ lệ kích hoạt, tiến hành kích hoạt một phòng ngẫu nhiên
        if (trigger <= triggerRate)
        {
            if (rooms.Length == 0)  // Kiểm tra nếu không có phòng nào có thể kích hoạt
            {
                Debug.LogWarning("No rooms available for triggering.");  // In cảnh báo nếu không có phòng
                return; // Thoát khỏi phương thức nếu không có phòng
            }

            // Chọn một phòng ngẫu nhiên từ mảng rooms
            int random = Random.Range(0, rooms.Length);
            rooms[random].RoomTrigger();  // Kích hoạt phòng được chọn
        }

        // Bắt đầu thời gian chờ sau khi kích hoạt xong
        _cooldown.StartCooldown();
    }
}
