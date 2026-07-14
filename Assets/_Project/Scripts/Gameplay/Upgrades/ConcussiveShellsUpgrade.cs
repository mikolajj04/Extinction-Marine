using ExtinctionMarine.Gameplay.Controllers;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public class ConcussiveShells : IUpgrade
    {
        public string Title => "[ Concussive Shells ]";
        public string Description => "Adds +2.5 kinetic impact to bullets, knocking enemies back.";

        public int CurrentLevel { get; set; } = 1;
        public int MaxLevel => 8;

        public void Apply(PlayerController player)
        {

            player.ApplyConcussiveShells();

        }
    }
}