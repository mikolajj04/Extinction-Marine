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

        public PlayerEntity() : base(100f, 5f)
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

        //for the future upgrade
        public void IncreaseDamage(float extraDamage)
        {
            if (IsDead) return;
            Damage += extraDamage;
        }

    }
}
