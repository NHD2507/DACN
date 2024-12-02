//using Unity.Netcode;
//using UnityEngine;

//public class InteractableSwitchLight : NetworkBehaviour, IInteractable
//{
//    [Header("Switch Settings")]
//    public GameObject switchON, switchOFF;
//    public GameObject lightorobj, breaker;

//    [Header("Audio Settings")]
//    public AudioSource audioSource;
//    public AudioClip sound1;

//    [Header("Power Settings")]
//    public bool IsPower = true; // Trạng thái mặc định của nguồn điện
//    public bool IsBreaker = true; // Trạng thái mặc định của breaker (có điện hay không)

//    private NetworkVariable<bool> syncedPowerState = new NetworkVariable<bool>(true);

//    public InteractionType GetInteractionType()
//    {
//        return InteractionType.PickUp; // Hoặc một loại tương tác phù hợp
//    }

//    public void Interact()
//    {
//        if (IsOwner)
//        {
//            TogglePowerServerRpc(!syncedPowerState.Value);
//        }
//    }

//    [ServerRpc(RequireOwnership = false)]
//    private void TogglePowerServerRpc(bool newPowerState)
//    {
//        syncedPowerState.Value = newPowerState;
//        HandleLightStateClientRpc(newPowerState);
//    }

//    [ClientRpc]
//    private void HandleLightStateClientRpc(bool powerState)
//    {
//        UpdateSwitchState(powerState);
//        LightOnOff(powerState);
//    }

//    private void UpdateSwitchState(bool powerState)
//    {
//        IsPower = powerState; // Cập nhật trạng thái cục bộ
//        switchON.SetActive(powerState);
//        switchOFF.SetActive(!powerState);

//        if (audioSource != null && sound1 != null)
//        {
//            audioSource.PlayOneShot(sound1);
//        }
//    }

//    public void LightOnOff(bool powerSource)
//    {
//        IsBreaker = breaker.GetComponent<InteractableBreaker>().GetPowerState(); // Lấy trạng thái breaker
//        lightorobj.SetActive(powerSource && IsBreaker);
//    }

//    private void Start()
//    {
//        // Đồng bộ trạng thái nguồn ban đầu
//        syncedPowerState.Value = IsPower;

//        syncedPowerState.OnValueChanged += (oldValue, newValue) =>
//        {
//            UpdateSwitchState(newValue);
//            LightOnOff(newValue);
//        };

//        // Cập nhật trạng thái hiển thị ban đầu
//        UpdateSwitchState(IsPower);
//        LightOnOff(IsPower);
//    }
//}

using Photon.Pun;
using UnityEngine;

public class InteractableSwitchLight : MonoBehaviour, IInteractable
{
    [Header("Switch Settings")]
    public GameObject switchON, switchOFF;
    public GameObject lightorobj, breaker;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip sound1;

    [Header("Power Settings")]
    public bool IsPower = true;
    public bool IsBreaker = true;

    private bool syncedPowerState = true;
    private PhotonView photonView;

    public InteractionType GetInteractionType()
    {
        return InteractionType.TogglePower;
    }

    public void Interact()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            TogglePower(!syncedPowerState);
        }
        else
        {
            photonView.RPC("RequestTogglePower", RpcTarget.MasterClient, !syncedPowerState);
        }
    }

    [PunRPC]
    private void RequestTogglePower(bool newPowerState)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            TogglePower(newPowerState);
            photonView.RPC("HandleLightState", RpcTarget.All, newPowerState);
        }
    }

    [PunRPC]
    private void HandleLightState(bool powerState)
    {
        syncedPowerState = powerState;
        UpdateSwitchState(powerState);
        LightOnOff(powerState);
    }

    private void TogglePower(bool newPowerState)
    {
        syncedPowerState = newPowerState;
    }

    private void UpdateSwitchState(bool powerState)
    {
        IsPower = powerState;
        switchON.SetActive(powerState);
        switchOFF.SetActive(!powerState);

        if (audioSource != null && sound1 != null)
        {
            audioSource.PlayOneShot(sound1);
        }
    }

    public void LightOnOff(bool powerSource)
    {
        if (breaker == null)
        {
            Debug.LogError("Breaker is not assigned.");
            return;
        }

        var interactableBreaker = breaker.GetComponent<InteractableBreaker>();
        if (interactableBreaker == null)
        {
            Debug.LogError("InteractableBreaker component is missing on the Breaker object.");
            return;
        }

        IsBreaker = interactableBreaker.GetPowerState();
        if (lightorobj == null)
        {
            Debug.LogError("Light object is not assigned.");
            return;
        }

        lightorobj.SetActive(powerSource && IsBreaker);
    }



    private void Start()
    {
        // Gán PhotonView nếu chưa gán
        photonView = GetComponent<PhotonView>();

        // Kiểm tra xem PhotonView đã được gán chưa
        if (photonView == null)
        {
            Debug.LogError("PhotonView is not attached.");
            return;
        }

        // Kiểm tra các đối tượng còn lại
        if (switchON == null || switchOFF == null || lightorobj == null || breaker == null)
        {
            Debug.LogError("One or more required objects are not assigned in the Inspector.");
            return;
        }

        UpdateSwitchState(IsPower);
        LightOnOff(IsPower);
    }



}
