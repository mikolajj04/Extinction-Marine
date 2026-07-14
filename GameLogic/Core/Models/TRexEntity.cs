using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class TRexEntity : DinosaurEntity
    {
        
        public TRexEntity() : base(8000f, 90f, 10.5f, 2000f)
        {
            IsImmuneToKnockback = true;
        }

        protected override void Die() { }
    }

}
