using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public abstract class DinosaurEntity : Entity
    {
        public float Speed { get; protected set; }
        public float XpReward { get; private set; }
        public bool IsImmuneToKnockback { get; protected set; } = false;
        public float MeleeKnockbackForce { get; protected set; } = 0f;
        public bool IsImpenetrable { get; protected set; } = false;
        public float Agility { get; protected set; } = 12f;
        public bool IsUsingSpecialAbility { get; protected set; } = false;


        protected DinosaurEntity(float maxHealth, float baseDamage, float speed, float xpReward)
            : base(maxHealth, baseDamage)
        {
            Speed = speed;
            XpReward = xpReward;
        }

        public virtual void Tick(float deltatime) { }

        public virtual void ResetEntity()
        {
            CurrentHealth = MaxHealth;
            IsUsingSpecialAbility = false;
        }
    }
}
