using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public abstract class DinosaurEntity : Entity
    {
        public float Speed { get; private set; }
        public float XpReward { get; private set; }

       
        protected DinosaurEntity(float maxHealth, float baseDamage, float speed, float xpReward)
            : base(maxHealth, baseDamage)
        {
            Speed = speed;
            XpReward = xpReward;
        }
    }
}
