using UnityEngine;

public class InteractableItem : MonoBehaviour, IInteractable
{
    public string itemName;
    public GameObject itemPrefab;
    public Transform rightHandSlot;
    public AudioSource audioSource;
    public AudioClip pickUpSound;

    private bool isPickedUp = false;

    public InteractionType GetInteractionType()
    {
        return InteractionType.PickUp;
    }

    public void Interact()
    {
        if (!isPickedUp)
        {
            PickUpItem();
        }
    }

    private void PickUpItem()
    {
        isPickedUp = true;

        GameObject pickedItem = Instantiate(itemPrefab, rightHandSlot.position, rightHandSlot.rotation);
        pickedItem.transform.SetParent(rightHandSlot);

        if (audioSource != null && pickUpSound != null)
        {
            audioSource.PlayOneShot(pickUpSound);
        }

        gameObject.SetActive(false);
        Debug.Log("Picked up: " + itemName);
    }
}
