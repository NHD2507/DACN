using Unity.Netcode;
using UnityEngine;

public class InteractableSwitchLight : NetworkBehaviour, IInteractable
{
    [Header("Switch Settings")]
    public GameObject switchON, switchOFF;
    public GameObject lightorobj, breaker;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip sound1;

    [Header("Power Settings")]
    public bool IsPower = true; // Trạng thái mặc định của nguồn điện
    public bool IsBreaker = true; // Trạng thái mặc định của breaker (có điện hay không)

    private NetworkVariable<bool> syncedPowerState = new NetworkVariable<bool>(true);

    public InteractionType GetInteractionType()
    {
        return InteractionType.PickUp; // Hoặc một loại tương tác phù hợp
    }

    public void Interact()
    {
        if (IsOwner)
        {
            TogglePowerServerRpc(!syncedPowerState.Value);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void TogglePowerServerRpc(bool newPowerState)
    {
        syncedPowerState.Value = newPowerState;
        HandleLightStateClientRpc(newPowerState);
    }

    [ClientRpc]
    private void HandleLightStateClientRpc(bool powerState)
    {
        UpdateSwitchState(powerState);
        LightOnOff(powerState);
    }

    private void UpdateSwitchState(bool powerState)
    {
        IsPower = powerState; // Cập nhật trạng thái cục bộ
        switchON.SetActive(powerState);
        switchOFF.SetActive(!powerState);

        if (audioSource != null && sound1 != null)
        {
            audioSource.PlayOneShot(sound1);
        }
    }

    public void LightOnOff(bool powerSource)
    {
        IsBreaker = breaker.GetComponent<InteractableBreaker>().GetPowerState(); // Lấy trạng thái breaker
        lightorobj.SetActive(powerSource && IsBreaker);
    }

    private void Start()
    {
        // Đồng bộ trạng thái nguồn ban đầu
        syncedPowerState.Value = IsPower;

        syncedPowerState.OnValueChanged += (oldValue, newValue) =>
        {
            UpdateSwitchState(newValue);
            LightOnOff(newValue);
        };

        // Cập nhật trạng thái hiển thị ban đầu
        UpdateSwitchState(IsPower);
        LightOnOff(IsPower);
    }
}
