using UnityEngine;

public class OpenCloseDoorMobile : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;
    public bool inReach, toggle;

    public Animator door;

    // Thêm biến để đánh dấu cửa đang được nhắm đến
    public static OpenCloseDoorMobile currentTarget;

    void Start()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.hideToggleUI();
        }
        else
        {
            Debug.LogWarning("UIManager không có trong ứng dụng");
        }

        inReach = false;
        toggle = true;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
            UIManager.Instance.showToggleUI();
            // Đặt cửa này làm cửa hiện tại
            currentTarget = this;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
            UIManager.Instance.hideToggleUI();
            // Đặt lại target nếu ra khỏi vùng
            if (currentTarget == this)
            {
                currentTarget = null;
            }
        }
    }

    public void HandleInteraction()
    {
        if (inReach)
        {
            if (toggle)
                DoorOpened();
            else
                DoorCloseed();

            UIManager.Instance.hideToggleUI();
        }
    }

    public void DoorOpened()
    {
        door.ResetTrigger("closed");
        door.SetTrigger("opened");
        audioSource.PlayOneShot(openSound);
        toggle = false;
    }

    public void DoorCloseed()
    {
        door.ResetTrigger("opened");
        door.SetTrigger("closed");
        audioSource.PlayOneShot(closeSound);
        toggle = true;
    }
}
