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

    //private void HandleInteraction()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
    //        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
    //        {
    //            Debug.Log("Ray hit: " + hit.collider.gameObject.name);
    //            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
    //            if (interactable != null)
    //            {
    //                ulong objectId = hit.collider.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
    //                InteractServerRpc(objectId); // Gui yeu cau lên server
    //            }
    //        }
    //    }
    //}

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

    //[ServerRpc]
    //private void InteractServerRpc(ulong objectId)
    //{
    //    var networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectId];
    //    if (networkObject != null)
    //    {
    //        IInteractable interactable = networkObject.GetComponent<IInteractable>();
    //        if (interactable != null)
    //        {
    //            interactable.Interact();
    //        }
    //    }
    //}

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
