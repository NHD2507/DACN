using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PhotonPhotograph : MonoBehaviourPunCallbacks
{
    public GameObject photograph;
    public GameObject Plight;
    public AudioSource audioSource;
    public AudioClip sound1;
    private bool PlayerInZone;
    private bool isActive = false;
    private GameObject Hand;
    public bool onhand;
    public GameObject photoReach;
    [SerializeField] public PhotoCD _photoCD;

    void Start()
    {
        photograph.SetActive(true);
        Plight.SetActive(false);
        PlayerInZone = false;
        onhand = false;
        photoReach.SetActive(false);
        Hand = GameObject.FindGameObjectWithTag("RightHand");
    }

    void Update()
    {
        if (!onhand)
        {
            onhand = photograph.GetComponent<OnHand>().onHand;
        }

        if (onhand == true)
        {
            photoReach.SetActive(true);
        }

        if (PlayerInZone && onhand && Input.GetKeyDown(KeyCode.F) && !_photoCD.IsOutOfUseTime)
        {
            if (!isActive)
            {
                photonView.RPC("ActivateFlashlightForOneSecond", RpcTarget.All);
                photonView.RPC("PlaySoundEffect", RpcTarget.All);
            }
        }

        _photoCD.UseTimeUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach") // If player in zone
        {
            PlayerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach") // If player exits the zone
        {
            PlayerInZone = false;
        }
    }

    [PunRPC]
    void ActivateFlashlightForOneSecond()
    {
        if (!isActive)
        {
            StartCoroutine(FlashlightRoutine());
        }
    }

    IEnumerator FlashlightRoutine()
    {
        Hand.GetComponent<Equip>().enabled = false;
        isActive = true;
        Plight.SetActive(true);

        yield return new WaitForSeconds(1f);

        Plight.SetActive(false);
        isActive = false;
        Hand.GetComponent<Equip>().enabled = true;
        _photoCD.DecreaseFlsUseTimes();
    }

    [PunRPC]
    void PlaySoundEffect()
    {
        audioSource.clip = sound1;
        audioSource.Play();
    }
}
