using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay;

namespace Assets._Project.Scripts.Gameplay.Upgrades
{
    public interface IUpgrade
    {
          string Title { get; }
          string Description { get; }
        void Apply(PlayerController player);
    }
}
