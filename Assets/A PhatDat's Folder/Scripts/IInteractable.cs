public enum InteractionType
{
    PickUp,
    OpenDoor,
    Inspect
}

public interface IInteractable
{
    InteractionType GetInteractionType(); // Tra ve loai tuong tac
    void Interact(); // Dinh nghia hanh dong tuong tac
}
