using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class StegosaurusEntity : DinosaurEntity
    {
        public StegosaurusEntity() : base(60f, 25f, 3f, 55f)
        {
            MeleeKnockbackForce = 200f;
        }

        protected override void Die()
        {

        }
    }
}
