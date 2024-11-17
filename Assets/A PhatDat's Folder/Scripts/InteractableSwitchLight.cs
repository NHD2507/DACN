using Unity.Netcode;
using UnityEngine;

public class InteractableSwitchLight : NetworkBehaviour, IInteractable
{
    public GameObject txtToDisplay; // Hiển thị văn bản UI
    private bool playerInZone; // Kiểm tra nếu người chơi trong vùng trigger

    public AudioSource audioSource;
    public AudioClip sound1;

    public GameObject switchON, switchOFF;
    public GameObject lightObj, breaker;

    private NetworkVariable<bool> isPower = new NetworkVariable<bool>(true); // Đồng bộ trạng thái nguồn điện
    private NetworkVariable<bool> isBreaker = new NetworkVariable<bool>(true); // Đồng bộ trạng thái cầu dao

    private bool previousPowerState; // Để phát hiện sự thay đổi trạng thái điện

    void Start()
    {
        breaker = GameObject.FindGameObjectWithTag("Breaker");
        playerInZone = false;
        txtToDisplay.SetActive(false);
        switchON.SetActive(true);
        switchOFF.SetActive(false);
    }

    void Update()
    {
        // Kiểm tra nếu người chơi trong khu vực và nhấn phím E
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            if (IsOwner) // Chỉ chủ sở hữu mới có thể thay đổi trạng thái
            {
                Interact(); // Gọi phương thức Interact để thực hiện hành động
            }
        }

        // Chỉ cập nhật trạng thái đèn nếu trạng thái nguồn điện thay đổi
        if (isPower.Value != previousPowerState)
        {
            previousPowerState = isPower.Value; // Cập nhật trạng thái điện trước đó
            UpdateLightState(isPower.Value, isBreaker.Value);
        }
    }

    // Phương thức thực hiện hành động tương tác
    public void Interact()
    {
        // Chuyển đổi trạng thái nguồn điện khi người chơi tương tác
        TogglePowerServerRpc(!isPower.Value); // Thay đổi trạng thái nguồn điện trên server
    }

    // Server RPC để chuyển đổi trạng thái nguồn điện
    [ServerRpc(RequireOwnership = false)]
    private void TogglePowerServerRpc(bool powerState)
    {
        // Cập nhật trạng thái nguồn điện trên server
        isPower.Value = powerState;

        // Đồng bộ trạng thái tới tất cả các client
        TogglePowerClientRpc(powerState);
    }

    // Client RPC để cập nhật trạng thái nguồn điện trên tất cả các client
    [ClientRpc]
    private void TogglePowerClientRpc(bool powerState)
    {
        switchON.SetActive(!switchON.activeSelf); // Chuyển đổi trạng thái công tắc
        switchOFF.SetActive(!switchOFF.activeSelf);

        if (audioSource != null && sound1 != null)
        {
            audioSource.clip = sound1;
            audioSource.Play(); // Phát âm thanh khi chuyển đổi công tắc
        }
    }

    // Logic trigger khi người chơi vào khu vực
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reach")) // Kiểm tra nếu người chơi vào khu vực
        {
            txtToDisplay.SetActive(true);
            playerInZone = true;
        }
    }

    // Logic trigger khi người chơi rời khỏi khu vực
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Reach")) // Kiểm tra nếu người chơi rời khu vực
        {
            playerInZone = false;
            txtToDisplay.SetActive(false);
        }
    }

    // Cập nhật trạng thái đèn dựa trên nguồn điện và trạng thái cầu dao
    private void UpdateLightState(bool powerState, bool breakerState)
    {
        isBreaker.Value = breaker.GetComponent<Breaker>().powerSource; // Cập nhật trạng thái cầu dao từ đối tượng Breaker

        // Chỉ bật đèn nếu nguồn điện bật và cầu dao hoạt động
        lightObj.SetActive(isBreaker.Value && powerState);
    }

    // Phương thức này trả về loại tương tác cho IInteractable
    public InteractionType GetInteractionType()
    {
        return InteractionType.Inspect; // Hoặc loại tương tác khác nếu cần thiết
    }
}
