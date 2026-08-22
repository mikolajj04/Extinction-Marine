using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class CarnotaurusEntity : DinosaurEntity
    {
        private bool isCharging = false;
        private float ChargeDuration = 2f;
        private float ChargeCooldown = 10f;
        private float timer;
        private float baseSpeed;
        public CarnotaurusEntity() : base(1000f, 70f, 6f, 500f)
        {
            IsImmuneToKnockback = true;
            IsImpenetrable = true;
            baseSpeed = Speed;
            timer = ChargeCooldown;
            isCharging = false;
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
                }
                else
                {
                    isCharging = true;
                    Speed = baseSpeed * 2f;
                    timer = ChargeDuration;
                }
            }
        }


        public void Tick()
        {

        }
        

        protected override void Die() { }
    }
}
