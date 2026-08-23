using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class CarnotaurusEntity : DinosaurEntity
    {
        
        private float chargeDuration = 3f;
        private float chargeCooldown = 6f;
        private float timer;
        private float baseSpeed;
        private float baseAgility;
        public CarnotaurusEntity() : base(1000f, 60f, 7f, 500f)
        {
            MeleeKnockbackForce = 300f;
            IsImmuneToKnockback = true;
            IsImpenetrable = true;
            baseSpeed = Speed;
            timer = chargeCooldown;
            baseAgility = Agility;
        }

        public override void Tick(float deltaTime) //Carnotaur Charge
        {
            timer -= deltaTime;
            if (timer <= 0)
            {
                if (IsUsingSpecialAbility)
                {
                    IsUsingSpecialAbility = false;
                    Speed = baseSpeed;
                    timer = chargeCooldown;
                    Agility = baseAgility;
                }
                else
                {
                    IsUsingSpecialAbility = true;
                    Speed = baseSpeed * 4.5f;
                    Agility= baseAgility * 0.05f;
                    timer = chargeDuration;
                }
            }
        }

        

        protected override void Die() { }

        public override void ResetEntity()
        {
            base.ResetEntity(); 
            timer = chargeCooldown;
            Speed = baseSpeed;
            Agility = baseAgility;
        }
    }
}
