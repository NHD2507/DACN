//using UnityEngine;
//using Unity.Netcode;

//public class InteractionController : NetworkBehaviour
//{
//    [Header("Interaction Settings")]
//    [SerializeField] private float interactionDistance = 3f;
//    [SerializeField] private LayerMask interactableLayer;

//    [Header("Pickup Settings")]
//    //[SerializeField] private Transform rightHandTransform; // Vị trí giữ đồ
//    [SerializeField] private NetworkObject rightHandNetworkObject; // Thay vì Transform
//    private GameObject currentHeldItem;

//    private Camera playerCamera;

//    private void Start()
//    {
//        if (IsOwner)
//        {
//            playerCamera = Camera.main;
//            if (playerCamera == null)
//            {
//                Debug.LogError("No MainCamera found! Ensure your camera is tagged as 'MainCamera'.");
//            }
//        }
//    }

//    private void Update()
//    {
//        if (IsOwner && playerCamera != null)
//        {
//            HandleInteraction();
//        }
//    }

//    private void HandleInteraction()
//    {
//        if (Input.GetKeyDown(KeyCode.E))
//        {
//            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
//            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
//            {
//                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
//                if (interactable != null)
//                {
//                    var interactionType = interactable.GetInteractionType();
//                    switch (interactionType)
//                    {
//                        case InteractionType.PickUp:
//                            HandlePickup(interactable, hit.collider.gameObject);
//                            break;
//                        case InteractionType.OpenDoor:
//                            HandleDoorInteraction(interactable);
//                            break;
//                    }
//                }
//            }
//        }
//    }

//    private void HandlePickup(IInteractable interactable, GameObject item)
//    {
//        if (currentHeldItem != null) return; // Đã cầm đồ, không cho phép nhặt thêm

//        var networkObj = item.GetComponent<NetworkObject>();
//        if (networkObj != null)
//        {
//            ulong objectId = networkObj.NetworkObjectId;
//            PickupServerRpc(objectId);
//        }
//    }

//    private void HandleDoorInteraction(IInteractable interactable)
//    {
//        interactable.Interact(); // Gọi hành động mở cửa
//    }

//    [ServerRpc]
//    private void PickupServerRpc(ulong objectId)
//    {
//        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out var networkObject))
//        {
//            var interactable = networkObject.GetComponent<IInteractable>();
//            if (interactable != null && interactable.GetInteractionType() == InteractionType.PickUp)
//            {
//                PickupItemClientRpc(networkObject.NetworkObjectId);
//            }
//        }
//    }

//    [ClientRpc]
//    private void PickupItemClientRpc(ulong objectId)
//    {
//        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out var networkObject))
//        {
//            var item = networkObject.gameObject;
//            currentHeldItem = item;

//            // Chuyển quyền sở hữu đồ vật cho người chơi
//            if (IsOwner)
//            {
//                item.GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.LocalClientId);
//            }

//            // Gán làm con của RightHandNetworkObject
//            item.transform.SetParent(rightHandNetworkObject.transform);
//            item.transform.localPosition = Vector3.zero; // Đặt tại vị trí gốc
//            item.transform.localRotation = Quaternion.identity;

//            // Vô hiệu hóa collider để tránh xung đột
//            var collider = item.GetComponent<Collider>();
//            if (collider != null) collider.enabled = false;
//        }
//    }


//    public void DropItem()
//    {
//        if (currentHeldItem != null)
//        {
//            currentHeldItem.transform.SetParent(null); // Gỡ làm con
//            currentHeldItem.GetComponent<Collider>().enabled = true; // Bật lại collider
//            currentHeldItem.GetComponent<NetworkObject>().RemoveOwnership(); // Trả quyền sở hữu
//            currentHeldItem = null;
//        }
//    }

//}


using UnityEngine;
using Unity.Netcode;

public class InteractionController : NetworkBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    private Camera playerCamera;

    private void Start()
    {
        if (IsOwner)
        {
            // Tìm camera chỉ khi là chủ sở hữu
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogError("No MainCamera found! Ensure your camera is tagged as 'MainCamera'.");
            }
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            if (playerCamera != null)
            {
                HandleInteraction();
            }
            else
            {
                Debug.LogWarning("PlayerCamera is null. Cannot handle interaction.");
            }
        }
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerCamera == null)
            {
                Debug.LogWarning("PlayerCamera is null. Cannot cast ray.");
                return;
            }

            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
            {
                Debug.Log("Ray hit: " + hit.collider.gameObject.name);
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    var networkObj = hit.collider.gameObject.GetComponent<NetworkObject>();
                    if (networkObj != null)
                    {
                        ulong objectId = networkObj.NetworkObjectId;
                        InteractServerRpc(objectId); // Gửi yêu cầu lên server
                    }
                    else
                    {
                        Debug.LogWarning("Object hit does not have a NetworkObject component.");
                    }
                }
                else
                {
                    Debug.Log("Object hit is not interactable.");
                }
            }
        }
    }

    [ServerRpc]
    private void InteractServerRpc(ulong objectId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out var networkObject))
        {
            IInteractable interactable = networkObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
        else
        {
            Debug.LogWarning($"Object with ID {objectId} not found.");
        }
    }
}

