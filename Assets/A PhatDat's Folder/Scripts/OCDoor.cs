using Unity.Netcode;
using UnityEngine;

public class OCDoor : NetworkBehaviour, IInteractable
{
    [Header("UI and Audio")]
    public GameObject txtDoor; // UI Text hiển thị khi trong tầm tương tác
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    [Header("Door Settings")]
    public Animator door; // Animator cửa
    private NetworkVariable<bool> doorState = new NetworkVariable<bool>(true); // true: cửa đóng, false: cửa mở

    private bool isPlayerInRange = false; // Để kiểm tra trạng thái tương tác cục bộ

    private void Start()
    {
        if (txtDoor != null) txtDoor.SetActive(false);

        // Đăng ký sự kiện khi trạng thái cửa thay đổi
        doorState.OnValueChanged += (oldState, newState) =>
        {
            if (newState) DoorCloseedLocal();
            else DoorOpenedLocal();
        };

        // Cập nhật trạng thái ban đầu
        UpdateDoorState(doorState.Value);
    }

    public InteractionType GetInteractionType()
    {
        return InteractionType.OpenDoor; // Xác định kiểu tương tác
    }

    public void Interact()
    {
        if (IsOwner) // Chỉ cho phép chủ sở hữu tương tác
        {
            ToggleDoorServerRpc();
        }
    }

    public void ToggleDoor()
    {
        if (IsOwner)
        {
            ToggleDoorServerRpc();
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void ToggleDoorServerRpc()
    {
        doorState.Value = !doorState.Value; // Đảo ngược trạng thái cửa
    }

    private void UpdateDoorState(bool isClosed)
    {
        if (isClosed) DoorCloseedLocal();
        else DoorOpenedLocal();
    }

    private void DoorOpenedLocal()
    {
        if (door != null)
        {
            door.ResetTrigger("closed");
            door.SetTrigger("opened");
        }

        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }

    private void DoorCloseedLocal()
    {
        if (door != null)
        {
            door.ResetTrigger("opened");
            door.SetTrigger("closed");
        }

        if (audioSource != null && closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }
    }
}
