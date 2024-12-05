using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonFlashLight : MonoBehaviourPunCallbacks
{
    public GameObject flashlight;
    public GameObject Flight;
    public AudioSource audioSource; // Add this
    public AudioClip sound1; // Add this
    public AudioClip sound2; // Add this
    private bool PlayerInZone;
    public bool onhand;
    private bool isLightOn = false; // Add this

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

        // Kiểm tra nếu người chơi trong vùng và nhấn phím F
        if (PlayerInZone && Input.GetKeyDown(KeyCode.F) && onhand)  // if in zone and press F key
        {
            photonView.RPC("ToggleLight", RpcTarget.All); // Call RPC to toggle light state across all players
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")  // if player enters zone
        {
            PlayerInZone = true;
        }
    }

    [PunRPC]
    void ToggleLight()
    {
        Flight.SetActive(!Flight.activeSelf);  // Toggle the flashlight light

        isLightOn = !isLightOn;  // Toggle the light state

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
