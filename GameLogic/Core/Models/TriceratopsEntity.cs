using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class TriceratopsEntity : DinosaurEntity
    {
        public TriceratopsEntity() : base(100f, 35f, 5.5f, 180f)
        {
            IsImmuneToKnockback = true;
            MeleeKnockbackForce = 480f;
        }

        protected override void Die()
        {
            
        }
    }
}
