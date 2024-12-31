using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseDoorMobile : MonoBehaviour
{
    [Header("Door Setting")]
    public AudioSource audioSource;        //audio source
    public AudioClip openSound;            //sound 1
    public AudioClip closeSound;           //sound 2
    public bool inReach, toggle;

    public Animator door;

    void Start()
    {
        // Kiểm tra nếu UIManager đã tồn tại 
        if (UIManager.Instance != null)
        {
            UIManager.Instance.hideToggleUI();
        }
        else
        {
            Debug.LogWarning("UIManager không có trong ứng dụng");
        }

        inReach = false; // Player không ở trong vùng       
        toggle = true;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Reach") // Nếu player trong vùng
        {
            inReach = true;
            UIManager.Instance.showToggleUI();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach") // Nếu player ra khỏi vùng
        {
            inReach = false;
            UIManager.Instance.hideToggleUI();
        }
    }

    // Được gọi từ TouchController khi nhấn nút Interact
    public void HandleInteraction()
    {
        if (inReach) // Chỉ thực hiện nếu player trong vùng
        {
            if (toggle)
                DoorOpened();
            else
                DoorCloseed();

            UIManager.Instance.hideToggleUI();
            inReach = false;
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
        audioSource.PlayOneShot(closeSound); // Phát âm thanh đóng cửa
        toggle = true;
    }
}
