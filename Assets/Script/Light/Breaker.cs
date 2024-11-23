using Unity.Netcode;
using UnityEngine;

public class Breaker : NetworkBehaviour, IInteractable
{
    private NetworkVariable<bool> powerSource = new NetworkVariable<bool>(true); // Đồng bộ trạng thái breaker
    public AudioSource audioSource;
    public AudioClip sound1;

    [SerializeField] private Animator breakerAnimator; // Animator của Breaker
    [SerializeField] private string onTrigger = "TurnOn"; // Tên Trigger cho trạng thái ON
    [SerializeField] private string offTrigger = "TurnOff"; // Tên Trigger cho trạng thái OFF

    private void Start()
    {
        // Cập nhật trạng thái khi bắt đầu (dành cho Client kết nối sau)
        UpdateBreakerAnimation(powerSource.Value);

        // Lắng nghe thay đổi trạng thái trên NetworkVariable
        powerSource.OnValueChanged += OnPowerSourceChanged;
    }

    private void OnDestroy()
    {
        powerSource.OnValueChanged -= OnPowerSourceChanged;
    }

    private void OnPowerSourceChanged(bool previousValue, bool newValue)
    {
        UpdateBreakerAnimation(newValue);
    }

    private void UpdateBreakerAnimation(bool isOn)
    {
        // Cập nhật trạng thái animation của breaker
        if (breakerAnimator != null)
        {
            // Gửi trigger cho Animator tùy theo trạng thái
            breakerAnimator.SetTrigger(isOn ? onTrigger : offTrigger);
        }

        Debug.Log($"Breaker is now {(isOn ? "ON" : "OFF")}");
    }

    public InteractionType GetInteractionType()
    {
        return InteractionType.PickUp; // Hoặc loại tương tác phù hợp
    }

    public void Interact()
    {
        if (IsOwner)
        {
            ToggleBreakerStateServerRpc(!powerSource.Value);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ToggleBreakerStateServerRpc(bool newPowerState)
    {
        powerSource.Value = newPowerState; // Cập nhật trạng thái
        HandleBreakerStateClientRpc(newPowerState); // Đồng bộ hóa âm thanh
    }

    [ClientRpc]
    private void HandleBreakerStateClientRpc(bool newState)
    {
        if (audioSource != null && sound1 != null)
        {
            audioSource.PlayOneShot(sound1);
        }
    }

    public bool GetPowerState()
    {
        return powerSource.Value;
    }
}
