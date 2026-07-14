using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class DiplodocusEntity : DinosaurEntity
    {
        public DiplodocusEntity() : base(400f, 35f, 4f, 310f)
        {
            IsImmuneToKnockback = true;
        }

        protected override void Die() { }
    }
}
