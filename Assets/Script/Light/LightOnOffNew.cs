using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnOffNew : MonoBehaviour
{

    private bool PlayerInZone;                  //check if the player is in trigger
    public AudioSource audioSource; // Add this
    public AudioClip sound1; // Add this

    public GameObject switchON, switchOFF;

    public GameObject lightorobj, breaker;

    public bool IsPower, IsBreaker;

    // Start is called before the first frame update
    void Start()
    {
        breaker = GameObject.FindGameObjectWithTag("Breaker");
        IsPower = true;
        PlayerInZone = false;                   //player not in zone       
                                                // Kiểm tra nếu UIManager đã tồn tại 
        if (UIManager.Instance != null)
        {
            UIManager.Instance.hideToggleUI();
        }
        else
        {
            Debug.LogWarning("UIManager không có trong ứng dụng");
        }

        switchON.SetActive(true);
        switchOFF.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInZone && Input.GetKeyDown(KeyCode.E))           //if in zone and press E key
        {
            IsPower = !IsPower;
            switchON.SetActive(!switchON.activeSelf);
            switchOFF.SetActive(!switchOFF.activeSelf);
            audioSource.clip = sound1; // Add this
            audioSource.Play(); // Add this
        }
        LightOnOff(IsPower);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")  //if player in zone
        {
            UIManager.Instance.showToggleUI();
            PlayerInZone = true;
        }
    }


    private void OnTriggerExit(Collider other)     //if player exit zone
    {
        if (other.gameObject.tag == "Reach")
        {
            PlayerInZone = false;
            UIManager.Instance.hideToggleUI();
        }
    }
    public void LightOnOff(bool powersource)
    {
        IsBreaker = breaker.GetComponent<Breaker>().powerSource;
        if (IsBreaker)
        {
            lightorobj.SetActive(powersource);
        }
        else
        {
            lightorobj.SetActive(false);
        }
    }
}
