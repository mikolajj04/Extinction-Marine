using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class CarnotaurusEntity : DinosaurEntity
    {
        
        private float chargeDuration = 2f;
        private float chargeCooldown = 5f;
        private float timer;
        private float baseSpeed;
        private float baseAgility;
        public CarnotaurusEntity() : base(1000f, 45f, 7f, 500f)
        {
            MeleeKnockbackForce = 350f;
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
                    Speed = baseSpeed * 6f;
                    Agility= baseAgility * 0.05f;
                    timer = chargeDuration;
                }
            }
        }

        


        public override void ResetEntity()
        {
            base.ResetEntity(); 
            timer = chargeCooldown;
            Speed = baseSpeed;
            Agility = baseAgility;
        }
    }
}
