using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class SpinosaurusEntity : DinosaurEntity
    {
        public SpinosaurusEntity() : base(2000f, 80f, 8f, 900f)
        {
            IsImmuneToKnockback = true;
        }

        protected override void Die() { }
    }
}
