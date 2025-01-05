//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FlashLight : MonoBehaviour
//{
//    public GameObject flashlight;
//    public GameObject Flight;
//    public AudioSource audioSource; // Add this
//    public AudioClip sound1; // Add this
//    public AudioClip sound2; // Add this
//    private bool PlayerInZone;
//    public bool onhand;
//    private bool isLightOn = false; // Add this
//    void Start()
//    { 
//        flashlight.SetActive(true);
//        Flight.SetActive(true);
//        PlayerInZone = false;
//        onhand = false;
//    }


//    void Update()
//    {
//        if (!onhand)

//            onhand = flashlight.GetComponent<OnHand>().onHand;
//        if (PlayerInZone && Input.GetKeyDown(KeyCode.F) && onhand)           //if in zone and press E key
//        {
//            Flight.SetActive(!Flight.activeSelf);
//            isLightOn = !isLightOn; // Toggle the light state

//            // Play the appropriate sound
//            if (isLightOn)
//            {
//                audioSource.clip = sound1;
//            }
//            else
//            {
//                audioSource.clip = sound2;
//            }
//            audioSource.Play();
//        }

//    }
//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.gameObject.tag == "Reach")     //if player in zone
//        {
//            PlayerInZone = true;
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public GameObject flashlight;
    public GameObject Flight;
    public AudioSource audioSource;
    public AudioClip sound1;
    public AudioClip sound2;
    private bool PlayerInZone;
    public bool onhand;
    private bool isLightOn = false;

    void Start()
    {
        flashlight.SetActive(true);
        Flight.SetActive(true);
        PlayerInZone = false;
        onhand = false;
    }

    void Update()
    {
        if (!onhand)
            onhand = flashlight.GetComponent<OnHand>().onHand;
    }

    public void ToggleLight()
    {
        if (onhand)           // check if on hand
        {
            Flight.SetActive(!Flight.activeSelf);
            isLightOn = !isLightOn;

            // Play the appropriate sound
            if (isLightOn)
            {
                audioSource.clip = sound1;
            }
            else
            {
                audioSource.clip = sound2;
            }
            audioSource.Play();
        }
    }
}
