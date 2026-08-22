using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class CarnotaurusEntity : DinosaurEntity
    {
        
        private bool isCharging = false;
        private float ChargeDuration = 3f;
        private float ChargeCooldown = 6f;
        private float timer;
        private float baseSpeed;
        private float baseAgility;
        public CarnotaurusEntity() : base(1000f, 60f, 7f, 500f)
        {
            MeleeKnockbackForce = 300f;
            IsImmuneToKnockback = true;
            IsImpenetrable = true;
            baseSpeed = Speed;
            timer = ChargeCooldown;
            isCharging = false;
            baseAgility = Agility;
        }

        public override void Tick(float deltaTime)
        {
            timer -= deltaTime;
            if (timer <= 0)
            {
                if (isCharging)
                {
                    isCharging = false;
                    Speed = baseSpeed;
                    timer = ChargeCooldown;
                    Agility = baseAgility;
                }
                else
                {
                    isCharging = true;
                    Speed = baseSpeed * 4.5f;
                    Agility= baseAgility * 0.05f;
                    timer = ChargeDuration;
                }
            }
        }

        

        protected override void Die() { }
    }
}
