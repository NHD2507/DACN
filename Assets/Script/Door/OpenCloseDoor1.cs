using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseDoor1 : MonoBehaviour
{
    [Header("Door Setting")]
    public AudioSource audioSource;        //audio source
    public AudioClip openSound;            //sound 1
    public AudioClip closeSound;           //sound 2
    public bool inReach, toggle;
    //check if the player is in trigger

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

        inReach = false;                   //player not in zone       
        toggle = true;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Reach")     //if player in zone
        {
            inReach = true;
            UIManager.Instance.showToggleUI();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")     //if player in zone
        {
            inReach = false;
            UIManager.Instance.hideToggleUI();
        }
    }
    // Update is called once per frame
    void Update()
    {
        openclosedoor();
    }

    void openclosedoor()
    {
        if (inReach && Input.GetKeyDown(KeyCode.E))           //if in zone and press E key
        {
            if (toggle) DoorOpened();
            else DoorCloseed();
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
        audioSource.PlayOneShot(closeSound);  //play close sound
        toggle = true;
    }
}
