using System.Collections;
using UnityEngine;

public class RoomID : MonoBehaviour
{
    [Header("Room Settings")]
    public GameObject room;
    public GameObject Switch; // Object quản lý trạng thái điện

    [Header("Dependencies")]
    public AnomalyID[] Anomalys; // Danh sách anomaly
    public OpenCloseDoor1 doorScript; // Script quản lý cửa

    [Header("Anomaly Settings")]
    public int lastValue; // Giá trị cuối cùng (không rõ mục đích, giữ nguyên)
    public int NumOfAnomaly; // Tổng số anomaly trong phòng
    public int AnomalyDifficultLevel; // Mức độ khó của anomaly
    public int NumberOfAnomalyTrigger; // Số anomaly sẽ kích hoạt

    private void Awake()
    {
        Debug.Log("RoomID Initialized");
        // Lấy tất cả anomaly từ các con của "room"
        Anomalys = room.GetComponentsInChildren<AnomalyID>();
        NumOfAnomaly = Anomalys.Length;
        NumberOfAnomalyTrigger = 1; // Giá trị mặc định, có thể tùy chỉnh
    }

    public void RoomTrigger()
    {
        // Kiểm tra nếu có script cửa và trạng thái cửa đóng
        if (doorScript != null && doorScript.IsOwner)
        {
            // Gửi lệnh đóng cửa từ server
            doorScript.ToggleDoor();
        }

        // Tắt nguồn điện
        if (Switch != null)
        {
            var lightScript = Switch.GetComponent<LightOnOffNew>();
            if (lightScript != null)
            {
                lightScript.IsPower = false;
                lightScript.LightOnOff(false);
            }
        }

        // Kích hoạt anomaly ngẫu nhiên
        for (int i = 0; i < NumberOfAnomalyTrigger; i++)
        {
            int random = Random.Range(0, Anomalys.Length); // Random anomaly
            Anomalys[random].Active(); // Kích hoạt anomaly
        }
    }
}
