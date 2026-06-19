using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public abstract class Entity
    {
        public float MaxHealth { get; private set;}
        public float CurrentHealth { get; private set; }

        public bool IsDead => CurrentHealth <= 0;

        protected Entity(float maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public virtual void TakeDamage(float amount)
        {
            if (IsDead) { return; }
            

            CurrentHealth -= amount;
            if (CurrentHealth < 0) { CurrentHealth = 0; }
            if(CurrentHealth == 0) { Die(); }

        }

        protected abstract void Die();

        public virtual void Heal(float amount)
        {
            if (CurrentHealth + amount < MaxHealth)
            {
                CurrentHealth += amount;
            }
            else
            {
                CurrentHealth = MaxHealth;
            }
        }


    }
}
