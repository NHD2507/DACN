using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTriggerNumber : MonoBehaviour
{
    public int TriggersNumber;          // Số lần kích hoạt cần đạt được để thực hiện hành động
    [SerializeField] private Cooldown _cooldown;  // Đối tượng Cooldown để kiểm tra thời gian chờ trước khi kiểm tra tiếp
    public GameObject TriggerAction;    // Đối tượng sẽ thực hiện hành động (ví dụ: Jumpscare) khi đủ số lần kích hoạt
    [Header("readonly")]
    public int TriggersCount;           // Biến đếm số lần kích hoạt đã xảy ra

    // Phương thức tăng số lần kích hoạt
    public void Increase() { TriggersCount++; }

    // Phương thức giảm số lần kích hoạt
    public void Decrease() { TriggersCount--; }

    // Phương thức Update() chạy mỗi frame
    public void Update()
    {
        if (_cooldown.IsCoolingDown) return;  // Nếu đang trong thời gian chờ, không thực hiện tiếp

        // Kiểm tra xem số lần kích hoạt đã đạt đến yêu cầu chưa
        if (TriggersCount == TriggersNumber)
        {
            if (TriggerAction != null)  // Nếu có đối tượng TriggerAction, thực hiện hành động
                TriggerAction.SetActive(true); // Kích hoạt hành động (ví dụ: Jumpscare)
            Debug.Log("Kích hoạt điều kiện thua");  // In ra thông báo khi điều kiện thua đã đạt
            this.enabled = false;  // Vô hiệu hóa script này để không kiểm tra lại
        }

        _cooldown.StartCooldown();  // Bắt đầu thời gian chờ trước khi kiểm tra lại
    }
}
