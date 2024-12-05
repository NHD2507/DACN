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

        // Tìm kiếm cameraReach và gán vào photoReach
        GameObject player = PhotonNetwork.LocalPlayer.TagObject as GameObject;
        if (player != null)
        {
            Transform cameraReach = player.transform.FindRecursive("CameraReach");
            if (cameraReach != null)
            {
                photoReach = cameraReach.gameObject;
                Debug.Log("CameraReach được gán thành công.");
            }
            else
            {
                Debug.LogWarning("cameraReach không được tìm thấy trong Player.");
            }
        }
        else
        {
            Debug.LogWarning("Không thể tìm thấy Player trong PhotonNetwork.LocalPlayer.");
        }

        // Chỉ gọi SetActive nếu photoReach đã được tìm thấy
        if (photoReach != null)
        {
            photoReach.SetActive(false);
        }
        else
        {
            Debug.LogWarning("photoReach vẫn chưa được gán, hãy kiểm tra logic tìm kiếm.");
        }
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
