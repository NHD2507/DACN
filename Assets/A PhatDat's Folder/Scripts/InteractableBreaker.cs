using UnityEngine;
using Photon.Pun;

public class InteractableBreaker : MonoBehaviour, IInteractable
{
    [SerializeField] private bool powerSource = true; // Trạng thái nguồn điện
    [SerializeField] private AudioSource audioSource; // Âm thanh
    [SerializeField] private AudioClip toggleSound; // Âm thanh bật/tắt nguồn

    // Reference tới PhotonView
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public InteractionType GetInteractionType()
    {
        return InteractionType.TogglePower; // Loại tương tác
    }

    public void Interact()
    {
        if (photonView.IsMine || PhotonNetwork.IsMasterClient)
        {
            // Chỉ chủ sở hữu hoặc Master Client mới được phép gửi tín hiệu
            photonView.RPC(nameof(TogglePower), RpcTarget.All, !powerSource);
        }
    }

    [PunRPC]
    private void TogglePower(bool newState)
    {
        powerSource = newState; // Cập nhật trạng thái nguồn điện
        PlayToggleSound(); // Phát âm thanh bật/tắt
        Debug.Log($"Breaker power source toggled: {powerSource}");
    }

    private void PlayToggleSound()
    {
        if (audioSource != null && toggleSound != null)
        {
            audioSource.PlayOneShot(toggleSound);
        }
    }

    // Thêm phương thức này để trả về trạng thái powerSource
    public bool GetPowerState()
    {
        return powerSource;
    }
}
