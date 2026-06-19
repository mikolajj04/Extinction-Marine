using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GameLogic.Core.Models
{
    public class PlayerEntity : Entity
    {
        public int Level { get; private set; }
        public float Experience { get; private set; }

        public PlayerEntity() : base(100f)
        {
            Level = 1;
            Experience = 0f;
        }


        public void AddExperience(float amount)
        {
            if (IsDead) { return; }
            Experience += amount;
        }

        protected override void Die()
        {

        }
        public void LevelUp()
        {
            Level++;
           
        }

     
    }
}
