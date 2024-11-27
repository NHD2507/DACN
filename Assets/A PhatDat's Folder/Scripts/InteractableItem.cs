using UnityEngine;
using Unity.Netcode;

public class InteractableItem : NetworkBehaviour, IInteractable
{
    public InteractionType GetInteractionType()
    {
        return InteractionType.PickUp;
    }

    public void Interact()
    {
        // Xử lý nhặt đồ (đã được quản lý trong InteractionController)
        Debug.Log("Item picked up!");
    }
}
