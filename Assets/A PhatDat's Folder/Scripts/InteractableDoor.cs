using UnityEngine;

public class InteractableDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator door;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private bool isOpen = false;

    public InteractionType GetInteractionType()
    {
        return InteractionType.OpenDoor;
    }

    public void Interact()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        door.SetTrigger("opened");
        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
        isOpen = true;
    }

    private void CloseDoor()
    {
        door.SetTrigger("closed");
        if (audioSource != null && closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }
        isOpen = false;
    }
}
