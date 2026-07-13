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
        public float MoveSpeed { get; private set; }
        public int PenetrationCount { get; private set; }
        public float ProjectileSpeed { get; private set; }
        public int ProjectileCount { get; private set; }
        public int RearProjectileCount { get; private set; }

        public PlayerEntity() : base(100f, 5f)
        {
            Level = 1;
            Experience = 0f;
            MoveSpeed = 8.5f;
            PenetrationCount = 1;
            ProjectileSpeed = 12f;
            ProjectileCount = 1;
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

        public void IncreasePenetration(int amount = 1)
        {
            if (IsDead) return;
            PenetrationCount += amount;
        }
        public void IncreaseDamage(float extraDamage)
        {
            if (IsDead) return;
            Damage += extraDamage;
        }
        public void IncreaseSpeed(float amount)
        {
            if (IsDead) return;
            MoveSpeed += amount;
        }

        public void IncreaseProjectileSpeed(float amount)
        {
            if (IsDead) return;
            ProjectileSpeed += amount;
            
        }

        public void AddProjectile()
        {
            if (IsDead) return;
            ProjectileCount++;
        }
        public void AddRearProjectile()
        {
            if (IsDead) return;
            RearProjectileCount++;
        }
    }
}
