using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnOffRadio : MonoBehaviour
{
    public bool PlayerInZone;         // Biến kiểm tra xem người chơi có trong khu vực tương tác không
    public AudioSource radio;         // Audio source phát ra âm thanh của radio
    public AudioSource on;            // Audio source phát ra âm thanh khi bật radio
    public AudioSource off;           // Audio source phát ra âm thanh khi tắt radio

    public bool IsOpened;             // Biến kiểm tra trạng thái của radio (đã bật hay chưa)

    // Phương thức khởi tạo ban đầu, được gọi trước khi frame đầu tiên chạy
    void Start()
    {
        StartCoroutine(ActivateRadioAfterFewSeconds());  // Bắt đầu coroutine để kích hoạt radio sau vài giây

        // Kiểm tra xem UIManager có tồn tại không
        if (UIManager.Instance != null)
        {
            UIManager.Instance.hideToggleUI();  // Ẩn UI toggle khi bắt đầu
        }
        else
        {
            Debug.LogWarning("UIManager không có trong ứng dụng");  // Cảnh báo nếu không tìm thấy UIManager
        }
    }

    // Phương thức Update, chạy mỗi frame
    void Update()
    {
        // Kiểm tra nếu người chơi ở trong khu vực và nhấn phím 'E'
        if (PlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            IsOpened = !IsOpened;  // Đảo ngược trạng thái của radio (bật/tắt)
            OnOffRadio();  // Gọi phương thức OnOffRadio để thực hiện hành động bật/tắt radio
        }
    }

    // Phương thức bật/tắt radio
    public void OnOffRadio()
    {
        if (IsOpened == true)  // Nếu radio đang bật
        {
            // Phát âm thanh khi bật radio
            if (on != null)
            {
                on.Play();  // Phát âm thanh bật radio
            }

            // Bắt đầu phát âm thanh của radio nếu radio chưa phát
            if (radio != null && !radio.isPlaying)
            {
                radio.Play();  // Phát âm thanh của radio
            }
        }
        else  // Nếu radio đang tắt
        {
            // Dừng phát âm thanh của radio và phát âm thanh tắt radio
            if (radio != null && radio.isPlaying)
            {
                radio.Stop();  // Dừng phát radio
            }

            if (off != null)
            {
                off.Play();  // Phát âm thanh tắt radio
            }
        }
    }

    // Phương thức gọi khi người chơi vào khu vực tương tác
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))  // Kiểm tra nếu đối tượng có tag "Reach"
        {
            UIManager.Instance.showToggleUI();  // Hiển thị UI toggle khi người chơi vào khu vực
            PlayerInZone = true;  // Đánh dấu là người chơi đang trong khu vực tương tác
        }
    }

    // Phương thức gọi khi người chơi rời khỏi khu vực tương tác
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))  // Kiểm tra nếu đối tượng có tag "Reach"
        {
            PlayerInZone = false;  // Đánh dấu là người chơi không còn trong khu vực
            UIManager.Instance.hideToggleUI();  // Ẩn UI toggle khi người chơi rời khỏi khu vực
        }
    }

    // Coroutine để kích hoạt radio sau vài giây
    IEnumerator ActivateRadioAfterFewSeconds()
    {
        IsOpened = false;  // Đặt radio ban đầu ở trạng thái tắt
        yield return new WaitForSeconds(10f);  // Chờ trong 10 giây
        IsOpened = true;  // Đặt radio ở trạng thái bật
        OnOffRadio();  // Gọi phương thức bật radio
    }
}
