namespace HackNSlash.Scripts.Player
{
    public interface IInteractable
    {
        public PlayerInteraction PlayerInteraction { get; set; }
        public void React();
    }
}