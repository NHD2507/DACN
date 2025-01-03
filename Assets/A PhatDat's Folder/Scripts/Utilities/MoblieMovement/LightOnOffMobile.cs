using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnOffMobile : MonoBehaviour
{
    // Các biến công khai để quản lý trạng thái, âm thanh, ánh sáng và các đối tượng
    public bool PlayerInZone;  // Trạng thái xem người chơi có trong khu vực tương tác không
    public AudioSource audioSource;  // Đối tượng phát âm thanh
    public AudioClip sound1;  // Âm thanh bật/tắt đèn

    public GameObject switchON, switchOFF;  // Các đối tượng hình ảnh cho nút bật/tắt đèn

    public GameObject lightorobj, breaker;  // Đối tượng ánh sáng và cầu dao (breaker)

    public bool IsPower, IsBreaker;  // Trạng thái điện nguồn và trạng thái của cầu dao

    void Start()
    {
        // Tìm đối tượng "Breaker" trong cảnh và gán vào biến breaker
        breaker = GameObject.FindGameObjectWithTag("Breaker");

        // Khởi tạo trạng thái
        IsPower = true;  // Mặc định điện là bật
        PlayerInZone = false;  // Người chơi chưa ở trong khu vực tương tác

        // Kiểm tra xem UIManager có tồn tại không, nếu có thì ẩn giao diện UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.hideToggleUI();
        }
        else
        {
            Debug.LogWarning("UIManager không có trong ứng dụng");  // Cảnh báo nếu UIManager không có
        }

        // Bật nút chuyển đổi đèn sáng
        switchON.SetActive(true);
        switchOFF.SetActive(false);
    }

    void Update()
    {
        // Kiểm tra nếu người chơi ở trong khu vực và nhấn phím 'E', thì chuyển đổi trạng thái đèn
        if (PlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            ToggleLight();  // Gọi phương thức để bật/tắt đèn
        }
    }

    // Phương thức được gọi khi có đối tượng vào trong khu vực trigger
    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu đối tượng vào khu vực trigger có tag "Reach"
        if (other.gameObject.tag == "Reach")
        {
            UIManager.Instance.showToggleUI();  // Hiển thị giao diện UI
            PlayerInZone = true;  // Cập nhật trạng thái người chơi ở trong khu vực
        }
    }

    // Phương thức được gọi khi có đối tượng ra khỏi khu vực trigger
    private void OnTriggerExit(Collider other)
    {
        // Kiểm tra nếu đối tượng ra khỏi khu vực trigger có tag "Reach"
        if (other.gameObject.tag == "Reach")
        {
            PlayerInZone = false;  // Cập nhật trạng thái người chơi ra khỏi khu vực
            UIManager.Instance.hideToggleUI();  // Ẩn giao diện UI
        }
    }

    // Phương thức điều khiển bật/tắt ánh sáng dựa vào trạng thái của nguồn điện
    public void LightOnOff(bool powersource)
    {
        // Lấy trạng thái nguồn điện từ cầu dao
        IsBreaker = breaker.GetComponent<Breaker>().powerSource;

        // Kiểm tra trạng thái của cầu dao, nếu có nguồn điện thì bật/tắt ánh sáng
        if (IsBreaker)
        {
            lightorobj.SetActive(powersource);  // Bật/tắt ánh sáng dựa vào trạng thái của powersource
        }
        else
        {
            lightorobj.SetActive(false);  // Nếu không có nguồn điện, tắt ánh sáng
        }
    }

    // Phương thức chuyển đổi trạng thái bật/tắt đèn
    public void ToggleLight()
    {
        // Đảo ngược trạng thái của nguồn điện
        IsPower = !IsPower;

        // Chuyển đổi trạng thái hiển thị của nút bật/tắt
        switchON.SetActive(!switchON.activeSelf);
        switchOFF.SetActive(!switchOFF.activeSelf);

        // Phát âm thanh khi bật/tắt đèn
        audioSource.clip = sound1;  // Chọn âm thanh để phát
        audioSource.Play();  // Phát âm thanh

        // Gọi phương thức điều khiển bật/tắt ánh sáng với trạng thái hiện tại
        LightOnOff(IsPower);
    }
}
