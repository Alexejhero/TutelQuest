using SchizoQuest.Game.Players;

namespace SchizoQuest.Interaction
{
    public interface IInteractable
    {
        bool CanInteract(Player player);
        void Interact(Player player);
    }
}
