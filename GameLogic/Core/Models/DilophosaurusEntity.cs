using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class DilophosaurusEntity : DinosaurEntity
    {
        private float timer;
        private float stealthDuration = 10f;
        private float stealthCooldown = 15f;
        public override float TargetAlpha => IsUsingSpecialAbility ? 0.1f : 1f;
        public DilophosaurusEntity() : base(35f, 25f, 11f, 70f)
        {
            IsSneaky = true;
            timer = stealthCooldown;
            AttackConeThreshold = 0f;

        }
        public override void Tick(float deltaTime)
        {
            timer -= deltaTime;
            if (timer <= 0)
            {
                if (IsUsingSpecialAbility)
                {
                    IsUsingSpecialAbility = false;
                    timer = stealthCooldown;
                }
                else
                {
                    IsUsingSpecialAbility = true;
                    timer = stealthDuration;
                }
            }
        }
    }
}

       
    

