using UnityEngine;
using Unity.Netcode;

public class InteractionController : NetworkBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private GameObject interactionHint;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    private Camera playerCamera;

    private void Start()
    {
        if (IsOwner)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            CheckForInteractable();
            HandleInteraction();
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
    //                interactable.Interact(); // Thực hiện hành động tương tác
    //            }
    //        }
    //    }
    //}

    private void CheckForInteractable()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactionHint.SetActive(true);
                return;
            }
        }
        interactionHint.SetActive(false);
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    ulong objectId = hit.collider.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
                    InteractServerRpc(objectId); // Gui yeu cau lên server
                }
            }
        }
    }

    [ServerRpc]
    private void InteractServerRpc(ulong objectId)
    {
        var networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectId];
        if (networkObject != null)
        {
            IInteractable interactable = networkObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
