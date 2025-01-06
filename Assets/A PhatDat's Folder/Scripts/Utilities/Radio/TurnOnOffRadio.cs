using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnOffRadio : MonoBehaviour
{
    private bool PlayerInZone;
    public AudioSource radio;        // Audio source
    public AudioSource on;        // Audio source
    public AudioSource off;        // Audio source

    public bool IsOpened;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivateRadioAfterFewSeconds());

        // Kiểm tra nếu UIManager đã tồn tại 
        if (UIManager.Instance != null)
        {
            UIManager.Instance.hideToggleUI();
        }
        else
        {
            Debug.LogWarning("UIManager không có trong ứng dụng");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInZone && Input.GetKeyDown(KeyCode.E))  // If in zone and press 'E' key
        {
            IsOpened = !IsOpened;
            OpenCloseRadio();
        }
    }

    public void OpenCloseRadio()
    {
        if (IsOpened)
        {
            // Play the sound for turning the radio on
            if (on != null)
            {
                on.Play();
            }

            // Start playing radio sound
            if (radio != null && !radio.isPlaying)
            {
                radio.Play();
            }
        }
        else
        {
            // Stop the radio sound and play the sound for turning the radio off
            if (radio != null && radio.isPlaying)
            {
                radio.Stop();
            }

            if (off != null)
            {
                off.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))  // If player enters the zone
        {
            UIManager.Instance.showToggleUI();
            PlayerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)  // If player exits the zone
    {
        if (other.gameObject.CompareTag("Reach"))
        {
            PlayerInZone = false;
            UIManager.Instance.hideToggleUI();
        }
    }

    // Coroutine to activate radio after a few seconds
    IEnumerator ActivateRadioAfterFewSeconds()
    {
        IsOpened = false;
        yield return new WaitForSeconds(60f);  // Wait for 60 seconds
        IsOpened = true;
        OpenCloseRadio();
    }
}
