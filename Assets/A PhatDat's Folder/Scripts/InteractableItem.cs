using Photon.Pun;
using UnityEngine;

public class InteractableItem : MonoBehaviour, IInteractable
{
    public InteractionType GetInteractionType()
    {
        return InteractionType.PickUp;
    }

    [PunRPC]
    public void Interact()
    {
        Debug.Log("Item picked up!");
    }
}
