//using Unity.Netcode;
//using UnityEngine;

//public class InteractableDoor : NetworkBehaviour, IInteractable
//{
//    [SerializeField] private Animator door;
//    [SerializeField] private AudioSource audioSource;
//    [SerializeField] private AudioClip openSound;
//    [SerializeField] private AudioClip closeSound;

//    private NetworkVariable<bool> isOpen = new NetworkVariable<bool>(false); // Đồng bộ trạng thái cửa

//    public InteractionType GetInteractionType()
//    {
//        return InteractionType.OpenDoor;
//    }

//    public void Interact()
//    {
//        // Chỉ client chủ sở hữu mới có thể tương tác và gửi tín hiệu tới server
//        if (IsOwner)
//        {
//            ToggleDoorServerRpc(!isOpen.Value);
//        }
//    }

//    [ServerRpc(RequireOwnership = false)]
//    private void ToggleDoorServerRpc(bool open)
//    {
//        isOpen.Value = open; // Cập nhật trạng thái trên server
//        HandleDoorStateClientRpc(open); // Đồng bộ trạng thái trên tất cả các client
//    }

//    [ClientRpc]
//    private void HandleDoorStateClientRpc(bool open)
//    {
//        if (open)
//        {
//            OpenDoor();
//        }
//        else
//        {
//            CloseDoor();
//        }
//    }

//    private void OpenDoor()
//    {
//        // Kích hoạt trigger "opened"
//        door.ResetTrigger("closed");
//        door.SetTrigger("opened");

//        // Phát âm thanh mở cửa
//        if (audioSource != null && openSound != null)
//        {
//            audioSource.PlayOneShot(openSound);
//        }

//        Debug.Log("Door opened");
//    }

//    private void CloseDoor()
//    {
//        // Kích hoạt trigger "closed"
//        door.ResetTrigger("opened");
//        door.SetTrigger("closed");

//        // Phát âm thanh đóng cửa
//        if (audioSource != null && closeSound != null)
//        {
//            audioSource.PlayOneShot(closeSound);
//        }

//        Debug.Log("Door closed");
//    }

//    private void Start()
//    {
//        // Lắng nghe sự thay đổi của `isOpen` để đồng bộ trạng thái cửa
//        isOpen.OnValueChanged += (oldValue, newValue) =>
//        {
//            if (newValue)
//            {
//                OpenDoor();
//            }
//            else
//            {
//                CloseDoor();
//            }
//        };
//    }
//}

using Photon.Pun; // Import thư viện Photon
using UnityEngine;

public class InteractableDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator door;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private bool isOpen = false; // Trạng thái cửa
    private PhotonView photonView;

    private void Start()
    {
        // Gắn PhotonView từ đối tượng hiện tại
        photonView = GetComponent<PhotonView>();
    }

    public InteractionType GetInteractionType()
    {
        return InteractionType.OpenDoor;
    }

    public void Interact()
    {
        // Chỉ client chủ sở hữu mới có thể gửi tín hiệu mở/đóng cửa
        if (photonView.IsMine)
        {
            photonView.RPC(nameof(ToggleDoor), RpcTarget.All, !isOpen);
        }
    }

    [PunRPC]
    private void ToggleDoor(bool open)
    {
        isOpen = open; // Cập nhật trạng thái cục bộ

        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        // Kích hoạt trigger "opened"
        door.ResetTrigger("closed");
        door.SetTrigger("opened");

        // Phát âm thanh mở cửa
        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }

        Debug.Log("Door opened");
    }

    private void CloseDoor()
    {
        // Kích hoạt trigger "closed"
        door.ResetTrigger("opened");
        door.SetTrigger("closed");

        // Phát âm thanh đóng cửa
        if (audioSource != null && closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }

        Debug.Log("Door closed");
    }
}

