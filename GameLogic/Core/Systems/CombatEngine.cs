using System;
using System.Collections.Generic;
using System.Text;
using GameLogic.Core.Models;

namespace GameLogic.Core.Systems
{
    public static class CombatEngine
    {
        public static void ResolveDamage<TAtacker, TTarger>(TAtacker atacker, TTarger target, float baseDamage)
        where TAtacker : Entity
        where TTarger : Entity
        {


            if (atacker.IsDead || target.IsDead) { return; }

            float finalDamage = baseDamage;

            target.TakeDamage(finalDamage);

        }




    }
}
